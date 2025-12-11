// /*
//  * MIT License
//  * Copyright (c) 2025 Alexander Gall
//  */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AugmeNDT
{
    public class CsvLoader : FileLoader
    {
        private CsvFileType csvFile;
        private List<List<string>> csvValues;

        private Encoding encoding;
        private char splitChar = ',';
        private int skipRows = IAbstractData.SkipRows;
        private bool hasSpatialValues = false;

        private bool automaticDetectionSuccesful = false;
        private Dictionary<string, List<string>> attributes = new Dictionary<string, List<string>>();

        public CsvLoader()
        {
            polyFiberDataset = ScriptableObject.CreateInstance<PolyFiberData>();
        }

        public CsvLoader(bool hasSpatialValues, int skipRows)
        {
            polyFiberDataset = ScriptableObject.CreateInstance<PolyFiberData>();
            this.skipRows = skipRows;
            this.hasSpatialValues = hasSpatialValues;
        }

        public override async Task LoadData(string filePath)
        {
            await ReadCsv(filePath);

            this.filePath = filePath;
            fileName = Path.GetFileNameWithoutExtension(filePath);

            datasetType = FileLoadingManager.DatasetType.Secondary;

            if (hasSpatialValues)
            {
                secondaryDataType = ISecondaryData.SecondaryDataType.Spatial;
                polyFiberDataset.FillPolyFiberData(csvValues);
                polyFiberDataset.SetDatasetName(fileName);
            }
            else
            {
                secondaryDataType = ISecondaryData.SecondaryDataType.Abstract;
                abstractDataset = csvFile.GetDataSet();
                abstractDataset.SetDatasetName(fileName);
            }
        }

        private async Task ReadCsv(string filePath)
        {
            Task<StreamReader> streamReaderTask = GetStreamReader(filePath);
            using var reader = await streamReaderTask;
            encoding = reader.CurrentEncoding;

            // Get Meta Infos in first line
            string metaLine = await reader.ReadLineAsync();
            string[] metaInfo = metaLine.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            ProcessMetaInfos(metaInfo);

            csvValues = new List<List<string>>();
            attributes.Clear(); // reset

            for (int skip = 0; skip < skipRows; skip++)
            {
                await reader.ReadLineAsync();
            }

            // Read header
            string headerLine = await reader.ReadLineAsync();
            string[] headerNames = headerLine.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

            if (headerNames == null || headerNames.Length < 1)
            {
                Debug.LogError("CSV File Header row is empty");
                return;
            }

            // Populate csvValues and attributes dictionary
            foreach (var name in headerNames)
            {
                var trimmedName = name.Trim();
                csvValues.Add(new List<string> { trimmedName });
                if (!attributes.ContainsKey(trimmedName))
                {
                    attributes.Add(trimmedName, new List<string>());
                }
            }

            // Read remaining rows
            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();
                string[] values = line.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

                if (values == null || values.Length < 1) continue;

                for (int feature = 0; feature < csvValues.Count; feature++)
                {
                    string val = feature < values.Length ? values[feature] : "";
                    csvValues[feature].Add(val);
                    attributes[csvValues[feature][0]].Add(val); // fill dictionary
                }
            }

            Debug.Log("CSV File loaded with attributes: " + string.Join(", ", attributes.Keys));

            csvFile = new CsvFileType(csvValues);
        }

        private void ProcessMetaInfos(string[] metaInfo)
        {
            if (metaInfo.Length < 1)
            {
                Debug.LogError("First line is empty");
                return;
            }
            switch (metaInfo[0])
            {
                case ISecondaryData.AbstractDataIdentifier:
                    automaticDetectionSuccesful = true;
                    hasSpatialValues = false;
                    skipRows = IAbstractData.SkipRows;
                    Debug.Log("Abstract Dataset detected");
                    break;
                case ISecondaryData.SpatialDataIdentifier:
                    automaticDetectionSuccesful = true;
                    hasSpatialValues = true;
                    skipRows = ISpatialData.SkipRows;
                    Debug.Log("Spatial Dataset [" + metaInfo[1] + "] detected");
                    break;
                default:
                    automaticDetectionSuccesful = false;
                    Debug.Log("CSV File has no valid meta information. Using abstract dataset as default");
                    break;
            }
        }

        public void PrintCsv()
        {
            StringBuilder csvOutput = new StringBuilder();

            for (int rowIndex = 0; rowIndex < csvValues[0].Count; rowIndex++)
            {
                csvOutput.Append("| ");
                for (int colIndex = 0; colIndex < csvValues.Count; colIndex++)
                {
                    csvOutput.Append(csvValues[colIndex][rowIndex] + "\t | ");
                }
                csvOutput.Append(" \n");
            }

            Debug.Log("CSV Output [" + encoding + "]: \n" + csvOutput);
        }

        public List<string> GetAllAttributeNames()
        {
            return attributes != null && attributes.Count > 0
                ? new List<string>(attributes.Keys)
                : new List<string>();
        }
    }
}
