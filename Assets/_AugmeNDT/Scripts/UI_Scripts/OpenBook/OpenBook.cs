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

        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip openBook;

        [SerializeField] GameObject openedBook;
        [SerializeField] GameObject insideBackCover;
        [SerializeField] GameObject BookSpine;
        [SerializeField] GameObject ClosedBook;


        private bool isCloseClicked;
        private Vector3 rotationVector;
        private bool isOpenClicked;
        private DateTime startTime;
        private DateTime endTime;

       
        void Start()
        {
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
                        gameObject.SetActive(false);
                        insideBackCover.SetActive(false);
                        openedBook.SetActive(true);
                        BookSpine.SetActive(false);
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
            isOpenClicked = true;
            
            gameObject.SetActive(false);
            insideBackCover.SetActive(false);
            openedBook.SetActive(true);
            BookSpine.SetActive(false);
            startTime = DateTime.Now;
            rotationVector = new Vector3(0, 180, 0);
            PlaySound();

        }

     
        public void closeBook_Click()
        {
            print("close book clicked in open book");


            isCloseClicked = true;
            startTime = DateTime.Now;
            rotationVector = new Vector3(0, -360, 0); // Rotate backward to close
            PlaySound();

            ClosedBook.SetActive(true);         // Show the closed book temporarily
            openedBook.SetActive(false);        // Hide the open book during animation
            insideBackCover.SetActive(true);    // Show the inside back cover
            BookSpine.SetActive(true);

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
