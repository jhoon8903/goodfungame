using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillCard : UI_Base
{
    #region Enums
    enum Texts
    {
        Name,
        Introduce,
        Price,
    }
    enum Images
    {
        IconBackground,
        IconImage,
        BuyBtn,

    }
    enum Buttons
    {
        BuyBtn,
    }

    #endregion

    #region Properties

    public SkillData Data { get; private set; }

    #endregion
    void Start()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));


        GetButton((int)Buttons.BuyBtn).gameObject.SetActive(true);
        GetButton((int)Buttons.BuyBtn).gameObject.BindEvent(PurchasePopup);

        Refresh();
        return true;
    }

    public void SetInfo(string key)
    {
        Data = Main.Data.Skills[key];
        Refresh();
    }

    public void Refresh()
    {
        if (Data == null) return;
        Init();
        GetText((int)Texts.Name).text = Data.skillStringKey;
        GetText((int)Texts.Introduce).text = Data.skillDesc;
        GetText((int)Texts.Price).text = $"{Data.skillPrice} Gold";
        GetImage((int)Images.IconImage).sprite = Main.Resource.Load<Sprite>($"{Data.skillStringKey}.sprite");


        if (Main.Game.PurchasedSkills.Contains(Data.skillStringKey))
        {
            // 이미 구매했음!
            GetButton((int)Buttons.BuyBtn).gameObject.SetActive(false);
            GetText((int)Texts.Price).text = "";
        }

        GetButton((int)Buttons.BuyBtn).interactable = Data.skillPrice < Main.Game.Gold; // 바인딩클릭막는건 이걸론 안된다
        GetImage((int)Images.BuyBtn).raycastTarget = Data.skillPrice < Main.Game.Gold; // 레이캐스트 끄니까 가능

    }
    void PurchasePopup(PointerEventData data)
    {
        Main.UI.ShowPopupUI<UI_Popup_Purchase>().SetInfo(this);
    }

}

