using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace RevitLauncher.Utils
{
	/// <summary> Represents an installed instance of Revit on the local machine. </summary>
	public class RevitProduct
	{
		/// <summary> The folder path in which .addin files associated to all users will be located for this product. </summary>
		public string AllUsersAddInFolder { get; private set; }

        /// <summary> The folder path in which .addin files associated to the current user will be located for this product. </summary>
		public string CurrentUserAddInFolder { get; private set; }

        /// <summary> The folder path where this Revit product is installed. </summary>
		public string InstallLocation { get; private set; }

        /// <summary> User-visible name for this Revit product. such as : 'Autodesk Revit Architecture 2010'. </summary>
		public string Name { get; private set; }

        /// <summary> The system architecture of this Revit product installation. </summary>
		public AddInArchitecture Architecture { get; } = AddInArchitecture.OS64bit;

        /// <summary> The id used to locate and identify the Revit product from the registry. </summary>
		public Guid ProductCode { get; private set; }

        /// <summary> The version for this Revit product. Such as: '2010'. </summary>
		public RevitVersion Version { get; }

        /// <summary> The release subversion. </summary>
		/// <since>
		///    2012
		/// </since>
		[CLSCompliant(false)]
		public uint Subversion { get; private set; }

        /// <summary> The product type. </summary>
		/// <since>
		///    2012
		/// </since>
		public ProductType Product { get; private set; } = ProductType.Unknown;

        /// <summary>The string for the release sub versions.</summary>
		/// <remarks>SubVersion releases may have additional APIs and functionality not available in the standard customer releases. Add-ins written to support standard Revit releases should be compatible with Subversion releases, but add-ins written specifically targeting new features in Subversion releases would not be compatible with the standard releases. </remarks>
		/// <since>
		///    2018
		/// </since>
		public string ReleaseSubVersion { get; private set; }

        /// <summary> Gets the installed language types. </summary>
		/// <returns>
		///    The installed language types.
		/// </returns>
		/// <since>
		///    2014
		/// </since>
		private ICollection<LanguageType> GetInstalledLanguages()
		{
			List<LanguageType> list = new List<LanguageType>();
			byte[] array = this.ProductCode.ToByteArray();
			byte[] array2 = array;
			int num = 6;
			array2[num] |= 1;
			foreach (LanguageType languageType in RevitProduct.sm_languageMap.Keys)
			{
				array[8] = RevitProduct.sm_languageMap[languageType][0];
				array[9] = RevitProduct.sm_languageMap[languageType][1];
				if (this.isGuidExistingInUninstallKeys(new Guid(array)))
				{
					list.Add(languageType);
				}
			}
			return list;
		}

		/// <summary> Default constructor </summary>
		internal RevitProduct()
		{
		}

		/// <summary> Constructor, set basic info for an Revit product. </summary>
		/// <param name="productCode"> Use to uniquely identify each product, it's a GUID. </param>
		/// <param name="product"> Use to identify product type. </param>
		/// <param name="architecture"> Architecture information, such as 32bit or 64bit. </param>
		/// <param name="version"> Define the version number, such as 2010, 2011. </param>
		internal RevitProduct(Guid productCode, ProductType product, AddInArchitecture architecture, RevitVersion version)
		{
			this.ProductCode = productCode;
			this.Product = product;
			this.Architecture = architecture;
			this.Version = version;
		}

		/// <summary>Gets the SubscriptionUpdate value from the registry. </summary>
		internal bool getSubscriptionValue()
		{
			bool result = false;
			string arg = RevitProductUtility.convertProductTypetoCode(this.Product);
			ICollection<LanguageType> installedLanguages = this.GetInstalledLanguages();
			foreach (LanguageType languageType in installedLanguages)
			{
				string arg2 = RevitProductUtility.convertLanguageTypeToCode(languageType);
				string name = string.Format("SOFTWARE\\Autodesk\\Revit\\{0}\\REVIT-{1}:{2}", this.Version.ToString().Replace("Revit", ""), arg, arg2);
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name);
				if (registryKey != null)
				{
					int num = 0;
					int num2 = (int)registryKey.GetValue("SubscriptionUpdate", num);
					if (num2 == 1)
					{
						result = true;
					}
					registryKey.Close();
				}
			}
			return result;
		}

		internal void SetProductCode(string productCode)
		{
			this.ProductCode = new Guid(productCode);
		}

		internal void SetInstallLocation(string value)
		{
			this.InstallLocation = value;
		}

		internal void SetName(string value)
		{
			this.Name = value;
		}

		internal void SetAllUsersAddInFolder(string value)
		{
			this.AllUsersAddInFolder = value;
		}

		internal void SetCurrentUserAddInFolder(string value)
		{
			this.CurrentUserAddInFolder = value;
		}

		internal void setSubversion(uint subversion)
		{
			this.Subversion = subversion;
		}

		internal void setReleaseSubVersion(string value)
		{
			this.ReleaseSubVersion = value;
		}

		internal void setLanguage(LanguageType languageType)
		{
			this.m_Language = languageType;
		}

		internal void setProduct(ProductType product)
		{
			this.Product = product;
		}

		private bool isGuidExistingInUninstallKeys(Guid value)
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall").OpenSubKey(value.ToString("B"));
			return registryKey != null;
		}

		static RevitProduct()
		{
			RevitProduct.sm_languageMap.Add(LanguageType.Chinese_Simplified, new byte[]
			{
				8,
				4
			});
			RevitProduct.sm_languageMap.Add(LanguageType.Chinese_Traditional, new byte[]
			{
				4,
				4
			});
			RevitProduct.sm_languageMap.Add(LanguageType.Czech, new byte[]
			{
				4,
				5
			});
			RevitProduct.sm_languageMap.Add(LanguageType.German, new byte[]
			{
				4,
				7
			});
			RevitProduct.sm_languageMap.Add(LanguageType.English_USA, new byte[]
			{
				4,
				9
			});
			RevitProduct.sm_languageMap.Add(LanguageType.Spanish, new byte[]
			{
				4,
				10
			});
			RevitProduct.sm_languageMap.Add(LanguageType.French, new byte[]
			{
				4,
				12
			});
			RevitProduct.sm_languageMap.Add(LanguageType.Hungarian, new byte[]
			{
				4,
				14
			});
			RevitProduct.sm_languageMap.Add(LanguageType.Italian, new byte[]
			{
				4,
				16
			});
			RevitProduct.sm_languageMap.Add(LanguageType.Japanese, new byte[]
			{
				4,
				17
			});
			RevitProduct.sm_languageMap.Add(LanguageType.Korean, new byte[]
			{
				4,
				18
			});
			RevitProduct.sm_languageMap.Add(LanguageType.Polish, new byte[]
			{
				4,
				21
			});
			RevitProduct.sm_languageMap.Add(LanguageType.Brazilian_Portuguese, new byte[]
			{
				4,
				22
			});
			RevitProduct.sm_languageMap.Add(LanguageType.Russian, new byte[]
			{
				4,
				25
			});
			RevitProduct.sm_languageMap.Add(LanguageType.English_GB, new byte[]
			{
				8,
				9
			});
		}

        private LanguageType m_Language = LanguageType.Unknown;

        private static IDictionary<LanguageType, byte[]> sm_languageMap = new Dictionary<LanguageType, byte[]>();
	}
}
