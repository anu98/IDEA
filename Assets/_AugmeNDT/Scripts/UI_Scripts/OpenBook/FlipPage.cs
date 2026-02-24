using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

namespace AugmeNDT

{
    public class FlipPage : MonoBehaviour
    {
     

        public PagePopulator pagePopulator;
        public void SetPagePopulator(PagePopulator populator)
        {
            pagePopulator = populator;
        }

        public enum ButtonType
        {
            forwardBtn1,
            forwardBtn,
            prevBtn
        }
    
        [SerializeField] PopulateIndexPage populateIndexPage;
        [SerializeField] Button ForwardButton;
        [SerializeField] Button ForwardButton1;
        [SerializeField] Button PrevButton;
        [SerializeField] GameObject PreviewsOpenButton;
        // [SerializeField] Button CloseButton;

        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip flipPage;
        [SerializeField] GameObject indexPage;

        public string folderpath;

        public int currentPageIndex = -1;
        private List<GameObject> pages = new List<GameObject>();

        private Vector3 rotationVector;
        private Vector3 startPosition;
        private Quaternion startRotation;
        private bool isClicked;


        private DateTime startTime;
        private DateTime endTime;
        // Start is called before the first frame update
        public void Start()
        {
            pagePopulator = GetComponent<PagePopulator>();

            if (pagePopulator == null)
            {
                Debug.LogError("PagePopulator component not found!");
                return;
            }
            if (string.IsNullOrEmpty(folderpath))
            {
                Debug.LogError("Folder path is not set!");
                return;
            }
            //pagePopulator.PopulatePages(folderpath);
            pages = pagePopulator.GetPages();
            if (pages.Count == 0)
            {
                Debug.LogError("No pages found!");
                return;
            }

            startRotation = transform.rotation;
            startPosition = transform.position;

            if (ForwardButton != null)
            {
                ForwardButton.onClick.RemoveAllListeners();
                ForwardButton.onClick.AddListener(() => TurnOnePage_Click(ButtonType.forwardBtn));
            }
            if (ForwardButton1 != null)
            {
                ForwardButton.onClick.RemoveAllListeners();
                ForwardButton1.onClick.AddListener(() => TurnOnePage_Click(ButtonType.forwardBtn));
            }
            else
            {
                Debug.Log("forward button null");
            }
            if (PrevButton != null)
            {
                ForwardButton.onClick.RemoveAllListeners();
                PrevButton.onClick.AddListener(() => TurnOnePage_Click(ButtonType.prevBtn));
            }
            else
            {
                Debug.Log("previous button null");
            }

            //if (CloseButton != null)
            //{
            //    CloseButton.onClick.AddListener(() => CloseBookBtn_Click());
            //}
            ShowCurrentPage();
        }

        // Update is called once per frame
        void Update()
        {
            if (isClicked)
            {
                transform.Rotate(rotationVector * Time.deltaTime);
                endTime = DateTime.Now;

                if ((endTime - startTime).TotalSeconds >= 1)
                {
                    isClicked = false;
                    transform.rotation = startRotation;
                    transform.position = startPosition;

                    ShowCurrentPage();
                }
            }
        }
        public void TurnOnePage_Click(ButtonType type)
        {

            //if(currentPageIndex <= -1)
            //{
            //    PrevButton.gameObject.SetActive(false);
            //}

            if (type == ButtonType.forwardBtn && currentPageIndex < pages.Count - 1)
            {
                currentPageIndex++;
            }
            else if (type == ButtonType.prevBtn && currentPageIndex > -1)
            {
                currentPageIndex--;
            }


            isClicked = true;
            startTime = DateTime.Now;

            if (type == ButtonType.forwardBtn)
            {
                Vector3 newRotation = new Vector3(startRotation.x, 180, startRotation.z);
                transform.rotation = Quaternion.Euler(newRotation);

                rotationVector = new Vector3(0, 180, 0);
            }
            else if (type == ButtonType.prevBtn)
            {

                rotationVector = new Vector3(0, -180, 0);

            }

            PlaySound();
        }

        public void ShowCurrentPage()
        {

            Debug.Log($"Current Page Index: {currentPageIndex}");
            //Hide all pages
            foreach (var page in pages)
            {
                page.SetActive(false);
            }
            // Hide index page by default
            if (indexPage != null)
                indexPage.SetActive(false);

            // Show index page if no page is selected
            if (currentPageIndex == -1)
            {
                indexPage.SetActive(true);
                //PreviewsOpenButton.SetActive(false);
            }

            // Show only the current page
            else if (currentPageIndex >= 0 && currentPageIndex < pages.Count)
            {
                pages[currentPageIndex].SetActive(true);
                var OpenButton = pages[currentPageIndex].transform.Find("Canvas/PreviewsOpenButton").gameObject;
                OpenButton.SetActive(true);
                // PreviewsOpenButton.SetActive(true);
                PagePopulator pagePopulator = GetComponent<PagePopulator>();
                if (pagePopulator == null)
                {
                    Debug.Log("null");
                }

                int datasetIndex = currentPageIndex;

                if (datasetIndex >= 0)
                    pagePopulator.GetFileInfoAtIndex(datasetIndex);

                //pagePopulator.GetFileInfoAtIndex(currentPageIndex);

            }
        }
        private void PlaySound()
        {
            if ((audioSource != null) && flipPage != null)
            {
                audioSource.PlayOneShot(flipPage);

            }
        }
       
            public void ResetBook()
            {
                currentPageIndex = -1;
                ShowCurrentPage();
            if (pages[currentPageIndex] == null)
            {
                Debug.LogWarning("Current page not ready yet");
                return;
            }
        }
        
        public void RefreshPages()
        {
            foreach (var p in pages)
            {
                Debug.Log(p.name + " active=" + p.activeSelf);
            }
            if (pagePopulator != null)
            {
                pages = pagePopulator.GetPages();
                Debug.Log($"Pages refreshed. Count = {pages.Count}");
            }
            else
            {
                Debug.LogWarning("PagePopulator is null in RefreshPages()");
            }
        }
        void ShowFirstPage()
        {
            if (pagePopulator == null || pagePopulator.pages.Count == 0) return;

            for (int i = 0; i < pagePopulator.pages.Count; i++)
                pagePopulator.pages[i].SetActive(i == 0);

            currentPageIndex = 0;
        }

        //private void CloseBookBtn_Click()
        //{
        //    AppEvent.CloseBookFunction();

        //}
        public void SetFolderPath(string path)
        {
            folderpath = path;
            Debug.Log($"SetFolderPath called in FlipPage: {folderpath}");

            // 1) Reset current page index
            currentPageIndex = -1;

            // 2) Clear old book pages
            if (pagePopulator != null)
                pagePopulator.ClearPages();

            // 3) Clear old index page content 
            if (populateIndexPage != null && populateIndexPage.contentPanel != null)
            {
                // Destroy all old dataset entries
                //foreach (Transform child in populateIndexPage.contentPanel)
                //    Destroy(child.gameObject);
                if (populateIndexPage != null && populateIndexPage.contentPanel != null)
                {
                    foreach (Transform child in populateIndexPage.contentPanel)
                    {
                        if (child != null && child.name.StartsWith("entriestext"))
                            child.gameObject.SetActive(false); // disable instead of destroy
                    }
                }

                // Set new folder path and repopulate index page
                populateIndexPage.setFolderPath(folderpath);
                populateIndexPage.PopulatePage(folderpath); // make PopulatePage public
            }
            else
            {
                Debug.LogWarning("populateIndexPage or its contentPanel is null!");
            }

            // 4) Populate new book pages
            if (pagePopulator != null)
                pagePopulator.PopulatePages(folderpath);

            // 5) Refresh runtime page list
            RefreshPages();

            // 6) Show index page
            ShowCurrentPage();
        }
        public void TurnNextPage()
        {
            Debug.Log("turnnextpage method called");
            TurnOnePage_Click(ButtonType.forwardBtn);
        }

        public void TurnPreviousPage()
        {
            Debug.Log("turnprevpage method called");

            TurnOnePage_Click(ButtonType.prevBtn);
        }


    }

}