using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


namespace AugmeNDT
{
    public class OpenBook : MonoBehaviour
    {
        [SerializeField] Button openBtn;
        [SerializeField] Button closebtn;
        [SerializeField] GameObject UserInterface;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip openBook;

        [SerializeField] GameObject openedBook;
        [SerializeField] GameObject ClosedBook;
        
        [SerializeField] GameObject insideBackCover;
        [SerializeField] GameObject BookSpine;
        [SerializeField] GameObject FrontofBook;

        private Quaternion closedRotation;
        public FlipPage flipPage;
        private bool isCloseClicked;
        private Vector3 rotationVector;
        private bool isOpenClicked;
        private DateTime startTime;
        private DateTime endTime;

       
        void Start()
        {
            closedRotation = transform.localRotation;
            if (openBtn != null)
            {
                openBtn.onClick.AddListener(() => openBtn_Click());
            }
            if (closebtn != null)
            {
                closebtn.onClick.AddListener(() => closeBook_Click());
            }
            // AppEvent.CloseBook += new EventHandler(closeBook_Click);
        }

     
        void Update()
        {
            if (isOpenClicked || isCloseClicked)
            {

                transform.Rotate(rotationVector * Time.deltaTime);
                endTime = DateTime.Now;

                if (isOpenClicked)
                {
                    if ((endTime - startTime).TotalSeconds >= 1)
                    {
                        isOpenClicked = false;
                        ClosedBook.SetActive(false);
                        FrontofBook.SetActive(false);
                        insideBackCover.SetActive(false);
                        BookSpine.SetActive(false);
                        openedBook.SetActive(true);
                    }
                }
                if (isCloseClicked && (endTime - startTime).TotalSeconds >= 1)
                {
                    isCloseClicked = false;
                    Debug.Log("Close animation completed; ClosedBook should now be visible.");
                }

            }
        }

        public void openBtn_Click()
        {
            transform.localRotation = closedRotation;
            PlaySound();
            // Hide closed visuals, show open
            ClosedBook.SetActive(false);
            FrontofBook.SetActive(false);
            insideBackCover.SetActive(false);
            BookSpine.SetActive(false);
            openedBook.SetActive(true);
         

        }

     
        public void closeBook_Click()
        {
            Debug.Log("close book clicked in open book");
            PlaySound();

            // Stop any close animation flag
            isCloseClicked = false;

            // Reset orientation so next open starts correctly
            transform.localRotation = closedRotation;
            if (flipPage != null)
                flipPage.ResetBook();
            openedBook.SetActive(false);
            // Show shelf UI
            UserInterface.SetActive(true);

            // Show closed book, hide open parts
            //ClosedBook.SetActive(true);
            
            //FrontofBook.SetActive(true);
            //insideBackCover.SetActive(true);
            //BookSpine.SetActive(true);
        }
        private void PlaySound()
        {
            if ((audioSource != null) && (openBook != null))
            {
                audioSource.PlayOneShot(openBook);
            }
        }
    }
}
