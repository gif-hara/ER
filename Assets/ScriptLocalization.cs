using UnityEngine;

namespace I2.Loc
{
	public static class ScriptLocalization
	{

		public static class Common
		{
			public static string Equipment 		{ get{ return LocalizationManager.GetTranslation ("Common/Equipment"); } }
			public static string Inventory 		{ get{ return LocalizationManager.GetTranslation ("Common/Inventory"); } }
			public static string System 		{ get{ return LocalizationManager.GetTranslation ("Common/System"); } }
		}
	}

    public static class ScriptTerms
	{

		public static class Common
		{
		    public const string Equipment = "Common/Equipment";
		    public const string Inventory = "Common/Inventory";
		    public const string System = "Common/System";
		}
	}
}