using System.Collections.Generic;
using UnityEngine;

namespace AugmeNDT
{
    public class OpenDatasetButton : MonoBehaviour
    {
        private SceneObjectHandler sceneObjectHandler;
        [SerializeField] GameObject ParameterSelectionMenu;

        [SerializeField] private AttributePopulator attributePopulator;
        [SerializeField] private GameObject PreviewButton;

        private void Awake()
        {
            // Auto-assign AttributePopulator if not set via inspector or Init()
            if (attributePopulator == null)
            {
                attributePopulator = FindObjectOfType<AttributePopulator>();
                if (attributePopulator == null)
                {
                    Debug.LogError("No AttributePopulator found in the scene! Please assign it in inspector or via Init().");
                }
            }
        }

        private void Start()
        {
            sceneObjectHandler = FindFirstObjectByType<SceneObjectHandler>();
            if (sceneObjectHandler == null)
            {
                Debug.LogError("SceneObjectHandler not found! Make sure GameManager has a reference to it.");
            }
        }

        /// <summary>
        /// Call this if you are instantiating the button dynamically and want to inject AttributePopulator
        /// </summary>
        public void Init(AttributePopulator populator)
        {
            attributePopulator = populator;
        }

        public async void OnPageClicked()
        {
            Transform pageTemplate = transform.parent.parent;
            Debug.Log("The clicked game object in pageBtnHandler: " + pageTemplate);

            PageData pageData = pageTemplate.GetComponent<PageData>();
            if (pageData == null)
            {
                Debug.LogError("PageData component not found on the clicked page!");
                return;
            }

            string path = pageData.GetFilePath();
            string correctedPath = path.Replace("\\", "/");

            Debug.Log("Calling SceneObjectHandler to load dataset from: " + correctedPath);
            DataVisGroup visGroup = await sceneObjectHandler.LoadPreSelectedObject(correctedPath);

            pageData.CachedGroup = visGroup;

            // Single call to unified preview factory
            DatasetPreviewFactory.CreatePreview(pageData, visGroup);
            ParameterSelectionMenu.SetActive(true);


            if (visGroup != null)
            {
                FindFirstObjectByType<VisChannelUpdater>()?.SetCurrentGroup(visGroup);

                // Get attribute names if CSV
                List<string> attributeNames = sceneObjectHandler.GetAttributeNamesOfLastLoadedCsv();
                if (attributeNames != null && attributeNames.Count > 0)
                {
                    Debug.Log("Attributes loaded: " + string.Join(", ", attributeNames));

                    if (attributePopulator == null)
                    {
                        Debug.LogError("AttributePopulator is STILL null!");
                    }
                    else
                    {
                        attributePopulator.Populate(attributeNames);
                    }
                }
                else
                {
                    Debug.Log("No CSV attributes available or loaded dataset is not CSV.");
                }
            }
            else
            {
                Debug.LogError("Failed to load dataset.");
            }
        }
    }
}