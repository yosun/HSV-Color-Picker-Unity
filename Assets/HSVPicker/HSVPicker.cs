﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class HSVPicker : MonoBehaviour {

	public HexRGB hexrgb;

    public static Color currentColor;
    public Image colorImage;
    public Image cursor;
    public RawImage hsvSlider;
    public RawImage hsvImage;

    //public InputField inputR;
    //public InputField inputG;
    //public InputField inputB;

	public Slider sliderRGB;
    public Slider sliderR;
    public Slider sliderG;
    public Slider sliderB;
    public Text sliderRText;
    public Text sliderGText;
    public Text sliderBText;

    public float pointerPos = 0;

    public float cursorX = 0;
    public float cursorY = 0;


//    public HSVSliderEvent onValueChanged = new HSVSliderEvent();

    private bool dontAssignUpdate = false;

    void Awake()
    {
        hsvSlider.texture = HSVUtil.GenerateHSVTexture((int)hsvSlider.rectTransform.rect.width, (int)hsvSlider.rectTransform.rect.height);

		sliderRGB.onValueChanged.AddListener(newValue =>
		{
			Color color=((Texture2D)hsvSlider.texture).GetPixel((int)(hsvSlider.rectTransform.rect.width*.5f),(int)(hsvSlider.rectTransform.rect.height*(newValue)));
			currentColor=color;
			if (!dontAssignUpdate)
			{
				AssignColor(currentColor);
			}
			hexrgb.ManipulateViaRGB2Hex();
        });
        sliderR.onValueChanged.AddListener(newValue =>
        {
            currentColor.r = newValue;
            if (!dontAssignUpdate)
            {
                AssignColor(currentColor);
            }
            sliderRText.text = "R:" + (int)(currentColor.r * 255f);
			hexrgb.ManipulateViaRGB2Hex();
        });
        sliderG.onValueChanged.AddListener(newValue =>
        {
            currentColor.g = newValue;
            if (!dontAssignUpdate)
            {
                AssignColor(currentColor);
            }
            sliderGText.text = "G:" + (int)(currentColor.g * 255f);
			hexrgb.ManipulateViaRGB2Hex();
        });
        sliderB.onValueChanged.AddListener(newValue =>
        {
            currentColor.b = newValue;
            if (!dontAssignUpdate)
            {
                AssignColor(currentColor);
            }
            sliderBText.text = "B:" + (int)(currentColor.b * 255f);
			hexrgb.ManipulateViaRGB2Hex();
        });

        
        hsvImage.texture = HSVUtil.GenerateColorTexture((int)hsvImage.rectTransform.rect.width, (int)hsvImage.rectTransform.rect.height, ((Texture2D)hsvSlider.texture).GetPixelBilinear(0, 0));
        MoveCursor(cursorX, cursorY);
	}
	

    public void AssignColor(Color color)
    {
        
		dontAssignUpdate=true;
        HsvColor hsv = HSVUtil.ConvertRgbToHsv(color);
		
        // Debug.Log(hsv.ToString());

        float hOffset = (float)(hsv.H / 360);

        //if (hsv.S > 1)
        //{
        //    hsv.S %= 1f;
        //}
        //if (hsv.V > 1)
        //{
        //    hsv.V %= 1f;
        //}

        MovePointer(hOffset, false);
        MoveCursor((float)hsv.S, (float)hsv.V, false);
        currentColor = color;
        UpdateInputs();
        colorImage.color = currentColor;

//        onValueChanged.Invoke(currentColor);
//z		hsvImage.texture = HSVUtil.GenerateColorTexture((int)hsvImage.rectTransform.rect.width, (int)hsvImage.rectTransform.rect.height, currentColor);		
        dontAssignUpdate=false;
    }
    
    
    public Color MoveCursor(float posX, float posY, bool updateInputs=true)
    {
//        dontAssignUpdate = updateInputs;
        if (posX > 1)
        {
            posX %= 1;
        }
        if (posY > 1)
        {
            posY %= 1;
        }

        posY = Mathf.Clamp(posY, 0, .9999f);
        posX = Mathf.Clamp(posX, 0, .9999f);
        

        cursorX = posX;
        cursorY = posY;
		cursor.rectTransform.anchoredPosition = new Vector2(posX * hsvImage.rectTransform.rect.width  , posY * hsvImage.rectTransform.rect.height - hsvImage.rectTransform.rect.height);

        currentColor = GetColor(cursorX, cursorY);
        colorImage.color = currentColor;

        if (updateInputs)
        {
            UpdateInputs();
//            onValueChanged.Invoke(currentColor);
        }
//        dontAssignUpdate = false;
        return currentColor;
    }

    public Color GetColor(float posX, float posY)
    {
        //Debug.Log(posX + " " + posY);
        return ((Texture2D)hsvImage.texture).GetPixel((int)(cursorX * hsvImage.texture.width ), (int)(cursorY * hsvImage.texture.height));
    }

    public Color MovePointer(float newPos, bool updateInputs = true)
    {
//        dontAssignUpdate = updateInputs;
        if (newPos > 1)
        {
            newPos %= 1f;//hsv
        }
        pointerPos = newPos;

        Color mainColor =((Texture2D)hsvSlider.texture).GetPixelBilinear(0, pointerPos);
        if (hsvImage.texture != null)
        {
            if ((int)hsvImage.rectTransform.rect.width != hsvImage.texture.width || (int)hsvImage.rectTransform.rect.height != hsvImage.texture.height)
            {
//                Destroy(hsvImage.texture);
//                hsvImage.texture = null;

                hsvImage.texture = HSVUtil.GenerateColorTexture((int)hsvImage.rectTransform.rect.width, (int)hsvImage.rectTransform.rect.height, mainColor);
            }
            else
            {
                HSVUtil.GenerateColorTexture(mainColor, (Texture2D)hsvImage.texture);
            }
        }
        else
        {

            hsvImage.texture = HSVUtil.GenerateColorTexture((int)hsvImage.rectTransform.rect.width, (int)hsvImage.rectTransform.rect.height, mainColor);
        }
        sliderRGB.value =  1f-pointerPos;

        currentColor = GetColor(cursorX, cursorY);
        colorImage.color = currentColor;

        if (updateInputs)
        {
            UpdateInputs();
//            onValueChanged.Invoke(currentColor);
        }
//        dontAssignUpdate = false;
        return currentColor;
    }

    public void UpdateInputs()
    {
		dontAssignUpdate=true;
        sliderR.value = currentColor.r;
        sliderG.value = currentColor.g;
        sliderB.value = currentColor.b;

        sliderRText.text = "R:"+ (currentColor.r * 255f);
        sliderGText.text = "G:" + (currentColor.g * 255f);
        sliderBText.text = "B:" + (currentColor.b * 255f);
        dontAssignUpdate=false;
        
    }

     void OnDestroy()
    {
        if (hsvSlider.texture != null)
        {
            Destroy(hsvSlider.texture);
        }

        if (hsvImage.texture != null)
        {
            Destroy(hsvImage.texture);
        }
    }
}
