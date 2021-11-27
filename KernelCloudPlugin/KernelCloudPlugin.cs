using Lumos.GUI;
using Lumos.GUI.Plugin;
using LumosLIB.Kernel.Log;

namespace KernelCloudPlugin
{
    public class KernelCloudPlugin : GuiPluginBase
    {
        /// <summary>
        /// Every Plugin has a unique ID
        /// </summary>
        const string PLUGIN_ID = "{BA54E029-4666-41a1-96C7-455CDF113903}";

        /// <summary>
        /// A logger to write Logfiles
        /// </summary>
        private static readonly ILumosLog log = LumosLogger.getInstance<KernelCloudPlugin>();


        private KernelCloudForm _form;

        public KernelCloudPlugin()
            : base(PLUGIN_ID, "Kernel Cloud Plugin")
        {
            
        }

        /// <summary>
        /// Called when Plugin is initialized (on Startup)
        /// </summary>
        protected override void initializePlugin()
        {
            this._form = new KernelCloudForm();   
        }

        /// <summary>
        /// Called when Plugin is disabled
        /// </summary>
        protected override void shutdownPlugin()
        {
            //Hide Form
            _form.Hide();
            //Remove Form, so it is not visible any more
            WindowManager.getInstance().RemoveWindow(_form);
        }

        /// <summary>
        /// Called when Plugin is activated (enabled in PluginManager)
        /// </summary>
        protected override void startupPlugin()
        {
            //Add Form
            WindowManager.getInstance().AddWindow(_form);
        }
    }
}
