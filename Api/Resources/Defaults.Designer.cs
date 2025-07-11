namespace Api.Resources;

/// <summary>
///   A strongly-typed resource class, for looking up localized strings, etc.
/// </summary>
// This class was auto-generated by the Visual Studio Code Extension PrateekMahendrakar.resxpress
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
public class Defaults 
{
	private static global::System.Resources.ResourceManager resourceMan;
	[global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
	public Defaults()
	{
	}
	/// <summary>
	///   Returns the cached ResourceManager instance used by this class.
	/// </summary>
	[global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
	public static global::System.Resources.ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(resourceMan, null))
			{
				global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Api.Resources.Defaults", typeof(Defaults).Assembly);
				resourceMan = temp;
			}
			return resourceMan;
		}
	}
	/// <summary>
	///   Overrides the current thread's CurrentUICulture property for all
	///   resource lookups using this strongly typed resource class.
	/// </summary>
	[global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
	public static global::System.Globalization.CultureInfo Culture { get; set; }
	
	/// <summary>
	/// Looks up a localized string similar to en-US.
	/// </summary>
	public static string English => ResourceManager.GetString("English", Culture);
	
	/// <summary>
	/// Looks up a localized string similar to fa-IR.
	/// </summary>
	public static string Persian => ResourceManager.GetString("Persian", Culture);
}
