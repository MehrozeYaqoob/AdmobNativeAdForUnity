using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System.Linq;

public class NativeAdsMethod : MonoBehaviour {

    private UnifiedNativeAd nativeAd;
    private bool unifiedNativeAdLoaded;

    // Below are the UI element that you need to assign from Unity Editor
    public GameObject adChoiceTexture;
    public GameObject appIcon;
    public GameObject headlines;
    public GameObject starRating;
    public GameObject store;
    public GameObject bodyText;
    public GameObject bigImage;
    public GameObject callToAction;
    public GameObject buttonText;

   
	// Use this for initialization
	void Start () {
        #if UNITY_ANDROID
        string appId = "ca-app-pub-3940256099942544~3347511713"; // Test App ID
        #elif UNITY_IPHONE
        string appId = "ca-app-pub-3940256099942544~1458002511";
        #else
        string appId = "unexpected_platform";
        #endif

        MobileAds.Initialize(appId);

        RequestNativeAd ();
	}
	
	// Update is called once per frame
    void Update () {
        if(this.unifiedNativeAdLoaded){
            this.unifiedNativeAdLoaded = false;

            Texture2D adChoiceLogoTexture = this.nativeAd.GetAdChoicesLogoTexture ();
            if(adChoiceLogoTexture!=null){
                adChoiceTexture.GetComponent<RawImage>().texture = adChoiceLogoTexture;
                if (!this.nativeAd.RegisterAdChoicesLogoGameObject(adChoiceTexture))
                {
                    Debug.Log("RegisterAdChoicesLogoGameObject Unsuccessfull");
                }
                adChoiceTexture.AddComponent<BoxCollider>();

            }

            Texture2D iconTexture = this.nativeAd.GetIconTexture ();
            if(iconTexture!=null){
                appIcon.GetComponent<RawImage>().texture = iconTexture;
                if (!this.nativeAd.RegisterIconImageGameObject(appIcon))
                {
                    Debug.Log("RegisterIconImageGameObject Unsuccessfull");
                }
                appIcon.AddComponent<BoxCollider>();
            }

            string headline = this.nativeAd.GetHeadlineText ();
            if(headline != null){
                headlines.GetComponent<Text> ().text = headline;
                if (!this.nativeAd.RegisterHeadlineTextGameObject(headlines))
                {
                    Debug.Log("RegisterHeadlineTextGameObject Unsuccessfull");
                }
                headlines.AddComponent<BoxCollider>();
            }

            string storeName = this.nativeAd.GetStore();
            if(storeName != null){
                store.GetComponent<Text> ().text = storeName;
                if (!this.nativeAd.RegisterStoreGameObject(store))
                {
                    Debug.Log("RegisterStoreGameObject Unsuccessfull");
                }
            }

            string bodyText = this.nativeAd.GetBodyText();
            if(bodyText != null){
                this.bodyText.GetComponent<Text> ().text = bodyText;
                if (!this.nativeAd.RegisterBodyTextGameObject(this.bodyText))
                {
                    Debug.Log("RegisterBodyTextGameObject Unsuccessfull");

                }
                this.bodyText.SetActive(false);
                this.bodyText.AddComponent<BoxCollider>();
            }

            double starRating = this.nativeAd.GetStarRating();
            if (starRating != null && starRating >= 0)
            {
                if(starRating >= 0 && starRating <2 ){this.starRating.transform.GetChild(0).gameObject.SetActive(true);}
                else if(starRating > 1 && starRating <3 ){this.starRating.transform.GetChild(1).gameObject.SetActive(true);}
                else if(starRating > 2 && starRating <4 ){this.starRating.transform.GetChild(2).gameObject.SetActive(true);}
                else if(starRating > 3 && starRating <5 ){this.starRating.transform.GetChild(3).gameObject.SetActive(true);}
                else if(starRating > 4 && starRating <6 ){this.starRating.transform.GetChild(4).gameObject.SetActive(true);}
            }
            else
            {
                this.starRating.SetActive(false);
                this.store.SetActive(false);
                this.bodyText.SetActive(true);
            }

            if (this.nativeAd.GetImageTextures().Count > 0)
            {
                List<Texture2D> goList = this.nativeAd.GetImageTextures();
                bigImage.GetComponent<RawImage>().texture = goList[0];
                List<GameObject> list = new List<GameObject>();
                list.Add(bigImage.gameObject);
                this.bigImage.AddComponent<BoxCollider>();

                
            }
            string buttonTextString = this.nativeAd.GetCallToActionText();
            if(buttonTextString != null){
                buttonText.GetComponent<Text> ().text = buttonTextString;
                this.buttonText.AddComponent<BoxCollider>();
            }
            if (!this.nativeAd.RegisterCallToActionGameObject(buttonText))
            {
                Debug.Log("RegisterCallToActionGameObject Unsuccessfull");
            }

        Debug.Log("Headline is "+headline);
        Debug.Log("Advitiser Text is "+ this.nativeAd.GetAdvertiserText());

        Debug.Log("GetBodyText is "+ this.nativeAd.GetBodyText());
        Debug.Log("GetCallToActionText is "+ buttonTextString);

        Debug.Log("GetPrice is "+ this.nativeAd.GetPrice());
        Debug.Log("GetStarRating is "+ starRating);
        Debug.Log("GetStore is "+ storeName);

        }
    }

    private void RequestNativeAd() {
        AdLoader adLoader = new AdLoader.Builder("ca-app-pub-3940256099942544/2247696110")
            .ForUnifiedNativeAd()
            .Build();
        adLoader.OnUnifiedNativeAdLoaded += this.HandleUnifiedNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;


        adLoader.LoadAd(new AdRequest.Builder().Build());
    }

    private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        Debug.Log("Native ad failed to load: " + args.Message);
    }

    private void HandleUnifiedNativeAdLoaded(object sender, UnifiedNativeAdEventArgs args) {
        Debug.Log("Unified Native Ad Loaded");
        this.nativeAd = args.nativeAd;
        this.unifiedNativeAdLoaded = true;
    }

    
}
