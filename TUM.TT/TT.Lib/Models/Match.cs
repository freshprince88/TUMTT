﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.6.81.0.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class Rank {
    
    private string positionField;
    
    private string dateField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Position {
        get {
            return this.positionField;
        }
        set {
            this.positionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Date {
        get {
            return this.dateField;
        }
        set {
            this.dateField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class Match {
    
    private MatchFirstPlayer firstPlayerField;
    
    private MatchSecondPlayer secondPlayerField;
    
    private MatchRally[] ralliesField;
    
    private string tournamentField;
    
    private string roundField;
    
    private string modeField;
    
    private string dateTimeField;
    
    private string videoFileField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public MatchFirstPlayer FirstPlayer {
        get {
            return this.firstPlayerField;
        }
        set {
            this.firstPlayerField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public MatchSecondPlayer SecondPlayer {
        get {
            return this.secondPlayerField;
        }
        set {
            this.secondPlayerField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    [System.Xml.Serialization.XmlArrayItemAttribute("Rally", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
    public MatchRally[] Rallies {
        get {
            return this.ralliesField;
        }
        set {
            this.ralliesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Tournament {
        get {
            return this.tournamentField;
        }
        set {
            this.tournamentField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Round {
        get {
            return this.roundField;
        }
        set {
            this.roundField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Mode {
        get {
            return this.modeField;
        }
        set {
            this.modeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string DateTime {
        get {
            return this.dateTimeField;
        }
        set {
            this.dateTimeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string VideoFile {
        get {
            return this.videoFileField;
        }
        set {
            this.videoFileField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class MatchFirstPlayer {
    
    private Rank rankField;
    
    private string nameField;
    
    private string nationalityField;
    
    private string spielsystemField;
    
    private string händigkeitField;
    
    private string griffhaltungField;
    
    private string materialField;
    
    /// <remarks/>
    public Rank Rank {
        get {
            return this.rankField;
        }
        set {
            this.rankField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Nationality {
        get {
            return this.nationalityField;
        }
        set {
            this.nationalityField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Spielsystem {
        get {
            return this.spielsystemField;
        }
        set {
            this.spielsystemField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Händigkeit {
        get {
            return this.händigkeitField;
        }
        set {
            this.händigkeitField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Griffhaltung {
        get {
            return this.griffhaltungField;
        }
        set {
            this.griffhaltungField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Material {
        get {
            return this.materialField;
        }
        set {
            this.materialField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class MatchSecondPlayer {
    
    private Rank rankField;
    
    private string nameField;
    
    private string nationalityField;
    
    private string spielsystemField;
    
    private string händigkeitField;
    
    private string griffhaltungField;
    
    private string materialField;
    
    /// <remarks/>
    public Rank Rank {
        get {
            return this.rankField;
        }
        set {
            this.rankField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Nationality {
        get {
            return this.nationalityField;
        }
        set {
            this.nationalityField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Spielsystem {
        get {
            return this.spielsystemField;
        }
        set {
            this.spielsystemField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Händigkeit {
        get {
            return this.händigkeitField;
        }
        set {
            this.händigkeitField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Griffhaltung {
        get {
            return this.griffhaltungField;
        }
        set {
            this.griffhaltungField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Material {
        get {
            return this.materialField;
        }
        set {
            this.materialField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class MatchRally {
    
    private MatchRallyCurrentRallyScore currentRallyScoreField;
    
    private MatchRallyCurrentSetScore currentSetScoreField;
    
    private MatchRallySchlag[] schlagField;
    
    private string nummerField;
    
    private string winnerField;
    
    private string lengthField;
    
    private string serverField;
    
    private string anfangField;
    
    private string endeField;
    
    private string kommentarField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public MatchRallyCurrentRallyScore CurrentRallyScore {
        get {
            return this.currentRallyScoreField;
        }
        set {
            this.currentRallyScoreField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public MatchRallyCurrentSetScore CurrentSetScore {
        get {
            return this.currentSetScoreField;
        }
        set {
            this.currentSetScoreField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Schlag", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public MatchRallySchlag[] Schlag {
        get {
            return this.schlagField;
        }
        set {
            this.schlagField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Nummer {
        get {
            return this.nummerField;
        }
        set {
            this.nummerField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Winner {
        get {
            return this.winnerField;
        }
        set {
            this.winnerField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Length {
        get {
            return this.lengthField;
        }
        set {
            this.lengthField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Server {
        get {
            return this.serverField;
        }
        set {
            this.serverField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Anfang {
        get {
            return this.anfangField;
        }
        set {
            this.anfangField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Ende {
        get {
            return this.endeField;
        }
        set {
            this.endeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Kommentar {
        get {
            return this.kommentarField;
        }
        set {
            this.kommentarField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class MatchRallyCurrentRallyScore {
    
    private string firstField;
    
    private string secondField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string First {
        get {
            return this.firstField;
        }
        set {
            this.firstField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Second {
        get {
            return this.secondField;
        }
        set {
            this.secondField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class MatchRallyCurrentSetScore {
    
    private string firstField;
    
    private string secondField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string First {
        get {
            return this.firstField;
        }
        set {
            this.firstField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Second {
        get {
            return this.secondField;
        }
        set {
            this.secondField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class MatchRallySchlag {
    
    private MatchRallySchlagSchlagtechnik schlagtechnikField;
    
    private MatchRallySchlagSpin spinField;
    
    private MatchRallySchlagPlatzierung platzierungField;
    
    private string nummerField;
    
    private string spielerField;
    
    private string schlägerseiteField;
    
    private string aufschlagartField;
    
    private string qualitätField;
    
    private string spielerpositionField;
    
    private string besonderesField;
    
    private string verlaufField;
    
    private string umlaufenField;
    
    private string balltreffpunktField;
    
    private string aggressivitätField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public MatchRallySchlagSchlagtechnik Schlagtechnik {
        get {
            return this.schlagtechnikField;
        }
        set {
            this.schlagtechnikField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public MatchRallySchlagSpin Spin {
        get {
            return this.spinField;
        }
        set {
            this.spinField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public MatchRallySchlagPlatzierung Platzierung {
        get {
            return this.platzierungField;
        }
        set {
            this.platzierungField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Nummer {
        get {
            return this.nummerField;
        }
        set {
            this.nummerField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Spieler {
        get {
            return this.spielerField;
        }
        set {
            this.spielerField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Schlägerseite {
        get {
            return this.schlägerseiteField;
        }
        set {
            this.schlägerseiteField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Aufschlagart {
        get {
            return this.aufschlagartField;
        }
        set {
            this.aufschlagartField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Qualität {
        get {
            return this.qualitätField;
        }
        set {
            this.qualitätField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Spielerposition {
        get {
            return this.spielerpositionField;
        }
        set {
            this.spielerpositionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Besonderes {
        get {
            return this.besonderesField;
        }
        set {
            this.besonderesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Verlauf {
        get {
            return this.verlaufField;
        }
        set {
            this.verlaufField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Umlaufen {
        get {
            return this.umlaufenField;
        }
        set {
            this.umlaufenField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Balltreffpunkt {
        get {
            return this.balltreffpunktField;
        }
        set {
            this.balltreffpunktField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Aggressivität {
        get {
            return this.aggressivitätField;
        }
        set {
            this.aggressivitätField = value;
        }
    }

    public bool IsTopLeft()
        {            
            double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
            double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

            return (X >= 0 && X < 50.5 && Y >= 0 && Y <= 46) || (X <= 152.5 && X > 102 && Y <= 274 && Y >= 228);
        }

    public bool IsTopMid()
    {
        double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
        double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

        return (X >= 50.5 && X <= 102 && Y >= 0 && Y <= 46) || (X >= 50.5 && X <= 102 && Y >= 228 && Y <= 274);
    }

    public bool IsTopRight()
    {
        double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
        double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

        return (X <= 152.5 && X > 102 && Y >= 0 && Y <= 46) || (X >= 0 && X < 50.5 && Y >= 228 && Y <= 274);
    }

    public bool IsMidLeft()
    {
        double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
        double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

        return (X >= 0 && X < 50.5 && Y <= 92 && Y > 46) || (X <= 152.5 && X > 102 && Y < 228 && Y >= 182);
    }
    public bool IsMidMid()
    {
        double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
        double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

        return (X >= 50.5 && X <= 102 && Y <= 92 && Y > 46) || (X >= 50.5 && X <= 102 && Y < 228 && Y >= 182);
    }
    public bool IsMidRight()
    {
        double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
        double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

        return (X <= 152.5 && X > 102 && Y <= 92 && Y > 46) || (X >= 0 && X < 50.5 && Y < 228 && Y >= 182);
    }

    public bool IsBotLeft()
    {
        double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
        double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

        return (X >= 0 && X < 50.5 && Y < 137 && Y > 92) || (X <= 152.5 && X > 102 && Y >= 137 && Y < 182);
    }
    public bool IsBotMid()
    {
        double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
        double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

        return (X >= 50.5 && X <= 102 && Y < 137 && Y > 92) || (X >= 50.5 && X <= 102 && Y >= 137 && Y < 182);
    }
    public bool IsBotRight()
    {
        double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
        double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

        return (X <= 152.5 && X > 102 && Y < 137 && Y > 92) || (X >= 0 && X < 50.5 && Y >= 137 && Y < 182);
    }

    public bool IsShort()
    {
        return this.Balltreffpunkt == "" ? false : this.Balltreffpunkt.ToLower() == "über";

    }

    public bool IsHalf()
    {

        return this.Balltreffpunkt == "" ? false : this.Balltreffpunkt.ToLower() == "hinter";

    }

    public bool IsLong()
    {


        return this.Balltreffpunkt == "" ? false : this.Balltreffpunkt.ToLower() == "halbdistanz";


    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class MatchRallySchlagSchlagtechnik {
    
    private string artField;
    
    private string optionField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Art {
        get {
            return this.artField;
        }
        set {
            this.artField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Option {
        get {
            return this.optionField;
        }
        set {
            this.optionField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class MatchRallySchlagSpin {
    
    private string usField;
    
    private string üsField;
    
    private string slField;
    
    private string srField;
    
    private string noField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string US {
        get {
            return this.usField;
        }
        set {
            this.usField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ÜS {
        get {
            return this.üsField;
        }
        set {
            this.üsField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string SL {
        get {
            return this.slField;
        }
        set {
            this.slField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string SR {
        get {
            return this.srField;
        }
        set {
            this.srField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string No {
        get {
            return this.noField;
        }
        set {
            this.noField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class MatchRallySchlagPlatzierung {
    
    private string wxField;
    
    private string wyField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string WX {
        get {
            return this.wxField;
        }
        set {
            this.wxField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string WY {
        get {
            return this.wyField;
        }
        set {
            this.wyField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class NewDataSet {
    
    private object[] itemsField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Match", typeof(Match))]
    [System.Xml.Serialization.XmlElementAttribute("Rank", typeof(Rank))]
    public object[] Items {
        get {
            return this.itemsField;
        }
        set {
            this.itemsField = value;
        }
    }
}
