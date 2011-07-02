using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DatabaseObjectSearcher;
using Microsoft.SqlServer.Management.UI.VSIntegration;


namespace HuntingDog
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />]
    /// 
    // f  {C454B5C8-3004-4893-B72A-A583E1789AD9}
    [Guid("B00DF00D-1234-1234-AAAA-BAADC0DE9991")]
	public partial class Connect : IDTExtensibility2, IDTCommandTarget
	{
          
		/// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
		}

        EventWatcher _eventWatcher;

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
            //CreateToolWindow();
            HuntingDog.DogEngine.StudioController.Current.CreateAddinWindow(_addInInstance);
            //MSSQLController.Current.CreateAddinWindow(_addInInstance);
        }

		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
          
            //return;          
            //_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;
        

            if (connectMode == ext_ConnectMode.ext_cm_AfterStartup) //ext_ConnectMode.ext_cm_UISetup
            {
                //MSSQLController.Current.CreateAddinWindow(_addInInstance);
            }

			if(connectMode == ext_ConnectMode.ext_cm_Startup) //ext_ConnectMode.ext_cm_UISetup
			{
              
                try
                {
                    _eventWatcher = new EventWatcher();
                    _eventWatcher.Attach(ServiceCache.ExtensibilityModel);
                }
                catch (Exception ex1)
                {
                    var a = ex1.Message;
                }

				object []contextGUIDS = new object[] { };
                Commands2 commands = (Commands2)ServiceCache.ExtensibilityModel.Commands;

               
				//Commands2 commands = (Commands2)_applicationObject.Commands;
				string toolsMenuName;

				try
				{
					//If you would like to move the command to a different menu, change the word "Tools" to the 
					//  English version of the menu. This code will take the culture, append on the name of the menu
					//  then add the command to that menu. You can find a list of all the top-level menus in the file
					//  CommandBar.resx.
                    ResourceManager resourceManager = new ResourceManager("HuntingDog.CommandBar", Assembly.GetExecutingAssembly()); //DIY: you will need to change this if you change the name of your project
					//CultureInfo cultureInfo = new System.Globalization.CultureInfo(_applicationObject.LocaleID);
                    CultureInfo cultureInfo = new CultureInfo(ServiceCache.ExtensibilityModel.LocaleID);
					string resourceName = String.Concat(cultureInfo.TwoLetterISOLanguageName, "Tools");
					toolsMenuName = resourceManager.GetString(resourceName);

				}
				catch
				{
					//We tried to find a localized version of the word Tools, but one was not found.
					//  Default to the en-US word, which may work for the current culture.
					toolsMenuName = "Tools";
				}
               
               
				//Place the command on the tools menu.
				//Find the MenuBar command bar, which is the top-level command bar holding all the main menu items:
				//Microsoft.VisualStudio.CommandBars.CommandBar menuBarCommandBar = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)["MenuBar"];
                Microsoft.VisualStudio.CommandBars.CommandBar menuBarCommandBar = ((Microsoft.VisualStudio.CommandBars.CommandBars)ServiceCache.ExtensibilityModel.CommandBars)["MenuBar"];

                var cb = (Microsoft.VisualStudio.CommandBars.CommandBars)ServiceCache.ExtensibilityModel.CommandBars;



				//Find the Tools command bar on the MenuBar command bar:
				CommandBarControl toolsControl = menuBarCommandBar.Controls[toolsMenuName];
				CommandBarPopup toolsPopup = (CommandBarPopup)toolsControl;

				//This try/catch block can be duplicated if you wish to add multiple commands to be handled by your Add-in,
				//  just make sure you also update the QueryStatus/Exec method to include the new command names.
				//try
				{
                    //DIY: this is what you need to do to change the icon of the addin - seriously - http://msdn2.microsoft.com/en-us/library/ms228771(VS.80).aspx
					//Add a command to the Commands collection:
                    
                    
                    //Command command = null;;
                    foreach (Command cmd in commands)
                        if (cmd.Name.Contains("HuntingDog") || cmd.Name.Contains("NavigateInOE"))
                        {
                            //toolsControl.Delete(cmd);
                            cmd.Delete();//
                            //throw new Exception();
                            break;
                        }

                    //Command command = null;;
                    foreach (Command cmd in commands)
                        if (cmd.Name.Contains("HuntingDog") || cmd.Name.Contains("NavigateInOE"))
                        {
                            //toolsControl.Delete(cmd);
                            cmd.Delete();//
                            //throw new Exception();
                            break;
                        } 
                  

                    var command = commands.AddNamedCommand2(_addInInstance, "HuntingDog", "Hunting Dog", "Unleash the dog!", true, 1, ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
                    
					//Add a control for the command to the tools menu:
                    if ((command != null) && (toolsPopup != null))
                    {

                        CommandBarControl control = (CommandBarControl)command.AddControl(toolsPopup.CommandBar, 1);
                        command.Bindings = "Global::ctrl+F3";


                        CommandBarButton button = (CommandBarButton)control;

                        var ole = (stdole.StdPicture)Microsoft.VisualBasic.Compatibility.VB6.Support.ImageToIPictureDisp(HuntingDog.Properties.Resources.footprint);
                        button.Picture = ole;//ImageTranslator.GetIPictureDisp(HuntingDog.Properties.Resources.footprint.GetHbitmap());

                    }


                    try
                    {
                        System.Threading.Thread.Sleep(70 * 1000);
                        var queryControl = cb["Query"];

                        var nvCmd = commands.AddNamedCommand2(_addInInstance, "NavigateInOE", "Navigate In Object Explorer", "Find object in object explorer", true, 1, ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
                        nvCmd.Bindings = "Global::ctrl+F2";

                        //CommandBarPopup queryPopup = (CommandBarPopup)queryControl;
                        CommandBarControl controlNav = (CommandBarControl)nvCmd.AddControl(queryControl, 1);
                       
                       
                        var ole = (stdole.StdPicture)Microsoft.VisualBasic.Compatibility.VB6.Support.ImageToIPictureDisp(HuntingDog.Properties.Resources.footprint);
                        (controlNav as CommandBarButton).Picture = ole;//ImageTranslator.GetIPictureDisp(HuntingDog.Properties.Resources.footprint.GetHbitmap());

                        // Add context command
                    }
                    catch { }
				}
				//catch(Exception ex)
				{
					//If we are here, then the exception is probably because a command with that name
					//  already exists. If so there is no need to recreate the command and we can 
                    //  safely ignore the exception.
				}

              
              

                //_applicationObject.Events.SelectionEvents.OnChange += new _dispSelectionEvents_OnChangeEventHandler(SelectionEvents_OnChange);
			}
		}


        /// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
        /// <param term='commandName'>The name of the command to determine state for.</param>
        /// <param term='neededText'>Text that is needed for the command.</param>
        /// <param term='status'>The state of the command in the user interface.</param>
        /// <param term='commandText'>Text requested by the neededText parameter.</param>
        /// <seealso class='Exec' />
        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {

            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                if (commandName.ToLower() == "HuntingDog.Connect.HuntingDog".ToLower()) //DIY: if you're changing the name of your addin you will need to change this
                {
                  
                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
            }
        }

        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
        {

            handled = false;
            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                if (commandName.ToLower() == "HuntingDog.Connect.HuntingDog".ToLower()) //DIY: if you're changing the name of your addin you will need to change this
                {

                    HuntingDog.DogEngine.StudioController.Current.CreateAddinWindow(_addInInstance);
                    //MSSQLController.Current.CreateAddinWindow(_addInInstance);
                    handled = true;
                    return;
                }
             
            }
        }

        void SelectionEvents_OnChange()
        {
            //throw new NotImplementedException();
        }

		/// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{

         
                 //if (disconnectMode != ext_DisconnectMode.ext_dm_UserClosed &&
                 //    disconnectMode != ext_DisconnectMode.ext_dm_HostShutdown)
                 //  return;
        

                 //// Remove all Commands
                 //Command cmd = null;
                 //try
                 //{
                 //    cmd =
                 // this._applicationObject.Commands.Item("AddinTemplate.Connect.FooAddinTemplate",
                 // -1);
                 //    cmd.Delete();
                 //    cmd =
                 // this._applicationObject.Commands.Item("AddinTemplate.Connect.BarAddinTemplate",
                 // -1);
                 //    cmd.Delete();
                 //}
                 //catch { ; }


		}

		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

	

        

		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
       
            if(MSSQLController.Current.SearchWindow!=null)
               MSSQLController.Current.SearchWindow.Visible = false;

        }
		


		/// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
		/// <param term='commandName'>The name of the command to execute.</param>
		/// <param term='executeOption'>Describes how the command should be run.</param>
		/// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
		/// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
		/// <param term='handled'>Informs the caller if the command was handled or not.</param>
		/// <seealso class='Exec' />
	

		//private DTE2 _applicationObject;
		private AddIn _addInInstance;
	}
}