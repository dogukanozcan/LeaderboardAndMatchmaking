using Naku.CoreSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Naku.AuthenticateSystem
{
    public class AuthManager : MonoSingleton<AuthManager>
    {
        public event Action OnSignedIn;
        public event Action OnSignedOut;
        [SerializeField] private bool _signInOnAwake = true;
        public bool IsSignedIn => AuthenticationService.Instance.IsSignedIn;

        private void Awake()
        {
            if (_signInOnAwake)
            {
                _ = SignInAnon();
            }
        }
        
        public async Task SignInAnon()
        {
            await UnityServices.InitializeAsync();

            if (AuthenticationService.Instance.IsSignedIn)
                return;

            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
                OnSignedIn?.Invoke();
            };
            AuthenticationService.Instance.SignInFailed += s =>
            {
                Debug.Log(s);
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }


        public void SignOut()
        {
            AuthenticationService.Instance.SignedOut += OnSignedOut;
            AuthenticationService.Instance.SignOut(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                if (AuthenticationService.Instance.IsSignedIn)
                {
                    SignOut();
                }
                else
                {
                    _ = SignInAnon();
                }
            }
        }
    }
}