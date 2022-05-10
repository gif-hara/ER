using UnityEngine;

namespace I2.Loc
{
	public static class ScriptLocalization
	{

		public static class Actor
		{
			public static string Name010101 		{ get{ return LocalizationManager.GetTranslation ("Actor/Name010101"); } }
			public static string Name020101 		{ get{ return LocalizationManager.GetTranslation ("Actor/Name020101"); } }
			public static string Name020200 		{ get{ return LocalizationManager.GetTranslation ("Actor/Name020200"); } }
		}

		public static class ArmorArm
		{
			public static string Name010101 		{ get{ return LocalizationManager.GetTranslation ("ArmorArm/Name010101"); } }
			public static string Name010102 		{ get{ return LocalizationManager.GetTranslation ("ArmorArm/Name010102"); } }
		}

		public static class ArmorHead
		{
			public static string Name010101 		{ get{ return LocalizationManager.GetTranslation ("ArmorHead/Name010101"); } }
			public static string Name010102 		{ get{ return LocalizationManager.GetTranslation ("ArmorHead/Name010102"); } }
		}

		public static class ArmorLeg
		{
			public static string Name010101 		{ get{ return LocalizationManager.GetTranslation ("ArmorLeg/Name010101"); } }
			public static string Name010102 		{ get{ return LocalizationManager.GetTranslation ("ArmorLeg/Name010102"); } }
		}

		public static class ArmorTorso
		{
			public static string Name010101 		{ get{ return LocalizationManager.GetTranslation ("ArmorTorso/Name010101"); } }
			public static string Name010102 		{ get{ return LocalizationManager.GetTranslation ("ArmorTorso/Name010102"); } }
		}

		public static class Common
		{
			public static string ArmorInformaiton 		{ get{ return LocalizationManager.GetTranslation ("Common/ArmorInformaiton"); } }
			public static string Empty 		{ get{ return LocalizationManager.GetTranslation ("Common/Empty"); } }
			public static string Equipment 		{ get{ return LocalizationManager.GetTranslation ("Common/Equipment"); } }
			public static string Inventory 		{ get{ return LocalizationManager.GetTranslation ("Common/Inventory"); } }
			public static string PickUp 		{ get{ return LocalizationManager.GetTranslation ("Common/PickUp"); } }
			public static string Rest 		{ get{ return LocalizationManager.GetTranslation ("Common/Rest"); } }
			public static string ShieldInformation 		{ get{ return LocalizationManager.GetTranslation ("Common/ShieldInformation"); } }
			public static string System 		{ get{ return LocalizationManager.GetTranslation ("Common/System"); } }
			public static string WeaponInformation 		{ get{ return LocalizationManager.GetTranslation ("Common/WeaponInformation"); } }
		}

		public static class Shield
		{
			public static string Name010101 		{ get{ return LocalizationManager.GetTranslation ("Shield/Name010101"); } }
			public static string Name010102 		{ get{ return LocalizationManager.GetTranslation ("Shield/Name010102"); } }
		}

		public static class ValuableItem
		{
			public static string Name010101 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010101"); } }
			public static string Name010102 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010102"); } }
			public static string Name010103 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010103"); } }
			public static string Name010104 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010104"); } }
			public static string Name010105 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010105"); } }
			public static string Name010106 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010106"); } }
			public static string Name010107 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010107"); } }
			public static string Name010108 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010108"); } }
			public static string Name010109 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010109"); } }
			public static string Name010110 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010110"); } }
			public static string Name010111 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010111"); } }
			public static string Name010112 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010112"); } }
			public static string Name010113 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010113"); } }
			public static string Name010114 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010114"); } }
			public static string Name010115 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010115"); } }
			public static string Name010116 		{ get{ return LocalizationManager.GetTranslation ("ValuableItem/Name010116"); } }
		}

		public static class Weapon
		{
			public static string Name010101 		{ get{ return LocalizationManager.GetTranslation ("Weapon/Name010101"); } }
			public static string Name010102 		{ get{ return LocalizationManager.GetTranslation ("Weapon/Name010102"); } }
			public static string Name010201 		{ get{ return LocalizationManager.GetTranslation ("Weapon/Name010201"); } }
			public static string Name010301 		{ get{ return LocalizationManager.GetTranslation ("Weapon/Name010301"); } }
			public static string Name020101 		{ get{ return LocalizationManager.GetTranslation ("Weapon/Name020101"); } }
			public static string Name020201 		{ get{ return LocalizationManager.GetTranslation ("Weapon/Name020201"); } }
			public static string Name020301 		{ get{ return LocalizationManager.GetTranslation ("Weapon/Name020301"); } }
		}
	}

    public static class ScriptTerms
	{

		public static class Actor
		{
		    public const string Name010101 = "Actor/Name010101";
		    public const string Name020101 = "Actor/Name020101";
		    public const string Name020200 = "Actor/Name020200";
		}

		public static class ArmorArm
		{
		    public const string Name010101 = "ArmorArm/Name010101";
		    public const string Name010102 = "ArmorArm/Name010102";
		}

		public static class ArmorHead
		{
		    public const string Name010101 = "ArmorHead/Name010101";
		    public const string Name010102 = "ArmorHead/Name010102";
		}

		public static class ArmorLeg
		{
		    public const string Name010101 = "ArmorLeg/Name010101";
		    public const string Name010102 = "ArmorLeg/Name010102";
		}

		public static class ArmorTorso
		{
		    public const string Name010101 = "ArmorTorso/Name010101";
		    public const string Name010102 = "ArmorTorso/Name010102";
		}

		public static class Common
		{
		    public const string ArmorInformaiton = "Common/ArmorInformaiton";
		    public const string Empty = "Common/Empty";
		    public const string Equipment = "Common/Equipment";
		    public const string Inventory = "Common/Inventory";
		    public const string PickUp = "Common/PickUp";
		    public const string Rest = "Common/Rest";
		    public const string ShieldInformation = "Common/ShieldInformation";
		    public const string System = "Common/System";
		    public const string WeaponInformation = "Common/WeaponInformation";
		}

		public static class Shield
		{
		    public const string Name010101 = "Shield/Name010101";
		    public const string Name010102 = "Shield/Name010102";
		}

		public static class ValuableItem
		{
		    public const string Name010101 = "ValuableItem/Name010101";
		    public const string Name010102 = "ValuableItem/Name010102";
		    public const string Name010103 = "ValuableItem/Name010103";
		    public const string Name010104 = "ValuableItem/Name010104";
		    public const string Name010105 = "ValuableItem/Name010105";
		    public const string Name010106 = "ValuableItem/Name010106";
		    public const string Name010107 = "ValuableItem/Name010107";
		    public const string Name010108 = "ValuableItem/Name010108";
		    public const string Name010109 = "ValuableItem/Name010109";
		    public const string Name010110 = "ValuableItem/Name010110";
		    public const string Name010111 = "ValuableItem/Name010111";
		    public const string Name010112 = "ValuableItem/Name010112";
		    public const string Name010113 = "ValuableItem/Name010113";
		    public const string Name010114 = "ValuableItem/Name010114";
		    public const string Name010115 = "ValuableItem/Name010115";
		    public const string Name010116 = "ValuableItem/Name010116";
		}

		public static class Weapon
		{
		    public const string Name010101 = "Weapon/Name010101";
		    public const string Name010102 = "Weapon/Name010102";
		    public const string Name010201 = "Weapon/Name010201";
		    public const string Name010301 = "Weapon/Name010301";
		    public const string Name020101 = "Weapon/Name020101";
		    public const string Name020201 = "Weapon/Name020201";
		    public const string Name020301 = "Weapon/Name020301";
		}
	}
}