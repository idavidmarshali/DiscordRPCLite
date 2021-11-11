<img src="https://cdn.discordapp.com/attachments/787696198693158912/908343727729303562/icon.ico" width="75" hight="75"/> ![Dotnet version 5.0](https://img.shields.io/badge/.NET%20Version-5.0-blue)



# DiscordRPCLite 

DiscordRPCLite is a lightweight program for setting **Custom Discord Rich Presences**

it has been written in C# and compiled for DotNet 5.x Desktop Runtime.

Report any problems by opening an Issue or sending me a message on [discord](https://discord.gg/Xa9VmvyyMw)

#### Sections :
- Releases:
  - [DRPCL-Lite](#drpcl-lite-release)
  - [DRPCL-StandAlone](#drpcl-standalone)

- Setup:
  - [Creating a discord App](#1-setting-up-a-discord-application)
  - [Setting Up programs Config](#2-setting-programs-configuration)
  - [Customizing the simple stuff](#3-customizing-the-rich-presences)
  - [Adding Buttons and Images](#4-adding-buttons-and-images)
  - [How to command](#5-commands)
- [For the Devs](#for-devs-or-nerds)

There are currently two releases available:

- [DRPCL-Lite](#drpcl-lite-release)
- [DRPCL-StandAlone](#drpcl-standalone)

### DRPCL-Lite Release
This is the actual **Lightweight** version which is just the compiled code + packages.

This release requires you to have [.Net 5.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet) installed
which is possibly already installed on your windows. 

(if you don't want to install the runtime, look at DRPCL-StandALone)

This release is ~500KB

### DRPCL-StandAlone
This is the main release which has the code + packages + runtime dlls needed to run the program
meaning there is no need for you to download .Net Runtime, all the required files are included
inside the program **at the cost of size**

(Your antivirus might see this as a virus, thats only cause its not signed!)

This release is ~12.5MB


# How to setup? :
using the program is simple and exactly the same in both releases.

First you have to create a discord application.

We're gonna do this step by step :

#### 1 Setting up a discord application
1. visit **[discord developer's portal](https://discord.com/developers)**
2. login to your discord account if your not logged in.
3. on the Top-Right side, click on the blue **New Application** button.
4. Enter a name for your application and hit **Create** (The name is going to be visible in your RichPresence)
5. copy the **application id** (its bellow the description field)
6. We're done with Dev Portal for now ..

#### 2 Setting Programs Configuration
1. Choose a release and download it.
2. Extract it, then enter DiscordRPCLite folder.
3. open the **config.json** file (pay attention to the name, dont open anyother config file)
4. now replace **ID Here** with the Application-ID you copied earlier. so it should look  something like `"ClientID" : "123456789"`

! The app is now ready to run, but first, lets do some modifications

![RPC Image](https://cdn.discordapp.com/attachments/787696198693158912/908309288655683604/unknown.png)

**Pay close attention to the picture!**

#### 3 Customizing the Rich Presences
1. Lets start with application name, you can't change that from the config file, you need to change this from the devportal. (change the name and click save)
2. Details - You can change it by changing the configs **Details** value like : `"Details" : "Some thing new"`
3. State - Same as Details
4. ShowTime - Can either be `"Yes"` or `"No"`, based on that, it will show the time elapsed from when the RPC has started.
5. LargImageKey/SmallImageKey - LargeImageText/SmallImageText -> we'll discuss this in the next part. for now re-run the program to see the changes happen. (for changing stuff on the fly, see the commands section)

#### 4 Adding Buttons and Images
1. For adding button, go into the config file and set Button1Text or Button2Text or both to what you want. then set there URLs, but be aware that the url has to start with `http://` or `https://` or else it will error. (**You can't leave the url empty**)
2. For images, first you need to visit the dev-portal again. then go into your applications page. in there click on the **rich presence** button on the middle-left. in the `Art Assets` section, and click on **Add Images** and select the image you want to use. rename it to a easy to use name like `Largimage` or sth. do this for both of the images you want to use as your Large and Small Image. Done? good. now head back to the config file and set `LargImageKey` to the name of the image you want to use as LargeImage like `"LargImageKey" : "name of the image in Art Assets tab"`.
Then you can either set a text for them or leave them empty, the text is visible when mouse hovers over the image.

#### 5 Commands
there are a just a couple of commands for the program. you can use them, after your rich presence has been update and the `>>` indecator is visible.

1. `>> help` - shows the help command of the program
2. `>> set`  - opens a menu which you can select and change the value of your rich presence (it won't save it to the config file).
3. `>> restart` - Restarts the client connection (not the program), configs wont be reloaded! *but i will add it in the next update*
4. `>> exit` - Exits the program and kills the client connection.

**THATS IT. you now have a beautiful rich presence just for you**

**tho you can share your config files with your friends so they could use the same rich presence too**


## For Devs or Nerds
The program can be built on `.Net Framework 3.5` all the way up to `.Net 5.0` (the code + dependencys are backward compatible)

Required Dependencys :
- [DiscordRichPresence by Laachy](https://github.com/Lachee/discord-rpc-csharp)
- [Newtonsoft.Json](https://www.newtonsoft.com/)

if you want to build it your self, clone the repo, get the dependencies, cd into the `/src` folder, then run the following command:
```cmd
>> dotnet build -o OUT_PUT_DIR /p:PublisheSingleFile=false /p:PublishTrimmed=false /p:RuntimeIdentifier=any
```
to build a platform independent version of the program (DRPCL-Lite)
