using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace Jufine.Backend.WebModel
{
	/// <summary>
	/// MasterPage Virtual File
	/// </summary>
	public class MasterPageVirtualFile : VirtualFile
	{
		private string virPath;

		/// <summary>
		/// Initializes a new instance of the <see cref="MasterPageVirtualFile"/> class.
		/// </summary>
		/// <param name="virtualPath">The virtual path to the resource represented by this instance.</param>
		public MasterPageVirtualFile(string virtualPath)
			: base(virtualPath)
		{
			this.virPath = virtualPath;
		}

		static Dictionary<string, Stream> resourceList = new Dictionary<string, Stream>();

		/// <summary>
		/// When overridden in a derived class, returns a read-only stream to the virtual resource.
		/// </summary>
		/// <returns>A read-only stream to the virtual file.</returns>
		public override Stream Open()
		{
			return ReadResource(virPath);
			//if (resourceList.ContainsKey(virPath))
			//{
			//    return resourceList[virPath];
			//}
			//else
			//{
				
			//    Stream stream = ReadResource(virPath);
			//    resourceList.Add(virPath, stream);
			//    return stream;
			//}
			//if (!(HttpContext.Current == null))
			//{
			//    if (HttpContext.Current.Cache[virPath] == null)
			//    {
			//        HttpContext.Current.Cache.Insert(virPath, ReadResource(virPath));
			//    }
			//    return (Stream)HttpContext.Current.Cache[virPath];
			//}
			//else
			//{
			//    return ReadResource(virPath);
			//}
		}
		private static Assembly currentAssembly;
		static MasterPageVirtualFile()
		{
			currentAssembly = Assembly.GetExecutingAssembly();
		}
		private static Stream ReadResource(string embeddedFileName)
		{
			String checkPath = VirtualPathUtility.ToAppRelative(embeddedFileName);
			string resourceFileName = checkPath.Replace(MasterPageVirtualPathProvider.VirtualMasterPageDir, string.Empty).Replace("/", ".");
			return currentAssembly.GetManifestResourceStream(MasterPageVirtualPathProvider.VirtualPathProviderResourceLocation + "." + resourceFileName);
		}
	}
}
