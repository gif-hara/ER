using UnityEngine;

namespace I2.Loc
{
	public static class ScriptLocalization
	{

		public static class Common
		{
			public static string ArmorInformaiton 		{ get{ return LocalizationManager.GetTranslation ("Common/ArmorInformaiton"); } }
			public static string Empty 		{ get{ return LocalizationManager.GetTranslation ("Common/Empty"); } }
			public static string Equipment 		{ get{ return LocalizationManager.GetTranslation ("Common/Equipment"); } }
			public static string Inventory 		{ get{ return LocalizationManager.GetTranslation ("Common/Inventory"); } }
			public static string ShieldInformation 		{ get{ return LocalizationManager.GetTranslation ("Common/ShieldInformation"); } }
			public static string System 		{ get{ return LocalizationManager.GetTranslation ("Common/System"); } }
			public static string WeaponInformation 		{ get{ return LocalizationManager.GetTranslation ("Common/WeaponInformation"); } }
		}
	}

    public static class ScriptTerms
	{

		public static class Common
		{
		    public const string ArmorInformaiton = "Common/ArmorInformaiton";
		    public const string Empty = "Common/Empty";
		    public const string Equipment = "Common/Equipment";
		    public const string Inventory = "Common/Inventory";
		    public const string ShieldInformation = "Common/ShieldInformation";
		    public const string System = "Common/System";
		    public const string WeaponInformation = "Common/WeaponInformation";
		}
	}
}