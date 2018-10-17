# Fall 2018 transWorld
A world that integrates all individual contributions.

## Version Required:
Unity 2018 2.9f1 --> upgraded to Unity 2018.2.12f1
(You can use Unity Hub to manage versions on your computer) -- [Go to page of Unity Hub](https://store.unity.com/download?ref=personal).

## Libraries Reference:
SteamVR 2.1

OSC

Raymarching

Volumetric Light

Kino

## Install Git (if you don't have it)
#### Windows: download and install
1. [Download Git for Windows](https://git-scm.com/downloads/win).
2. Install Git-2.xx-32/64.exe
3. done

#### Mac: Install homebrew and Command Line Tool (if you don't have), then install git using package manager
- Homebrew is a package manager that allows you to install different softwares in terminal
1. Open your terminal, copy paste:
```shell
/usr/bin/ruby -e "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install)"
```
2. If you have never used command line tool / XCode on Mac, you will be asked to install it from Apple, confirm install. Otherwise, skip this step
3. Use brew to install anything in terminal like this:
```shell
brew install git
```
4. done

#### IMPORTANT: Install Git LFS 
1. download GIT LFS at: https://git-lfs.github.com/
2. Install package
3. In your terminal (Mac) or powershell / Gitbash / cmd (Windows), type:
```shell
git lfs install
```
4. done

## git clone / pull / push
#### git clone
For first time or need a clean repo:
1. In terminal, use "cd your/folder/name" or cd .. to navigate into "transWorld" folder:
2. type:
```shell
git clone https://github.com/Translab/transWorld
```

#### git pull
Note: Everytime when you make changes to the project, please pull the latest version from the server
1. Mac OS: open your terminal / Windows: Powershell
2. cd your/path/transWorld or cd .. to navigate into "transWorld" folder
3. git pull 


#### git push (add, commit, push)
Push your contributions to the world every time you finishes:
In terminal:
```shell
cd your/path/transWorld
git add .  
```
this prepares and adds all changed files to be uploaded *notice the space and dot after “add”.
if you have only a few files to add:
```shell
git add filenameA
git add filenameB
```
when you finish adding, you should commit the changes you made and push to the master branch:
```shell
git commit -m “your_message”
git push origin master
```

#### conflict reports or errors when pull or push
The principle is that, you should make sure your local repo is exactly same as the server repo.

if you have issues
you can always use:
```shell
git stash
```
It removes all the local changes and makes the local repo exactly same as the previous pull. Then you can pull again from the server to get the latest version. 

*note: don't use this method if you have made significant changes already on the project, git stash will wipe out every changes you made.

## A brief guideline on how to start and edit the scene in Unity:
#### PlayGround
"PlayGround" is the collective main scene that will load and show everyone's contribution, however, we don't directly add objects to this scene, unless your script is dependent on certain objects in the PlayGround, such as CameraRig, terrain, or NetworkManager.

#### Additive Scene
We use a method called "additive scene" in Unity, that we load our own custom scene on top of the PlayGround main scene. You should create a scene with YOUR NAME, and put all your objects, models, audios in there. Then you can go to the PlayGround and drag drop your scene into the main scene. This will make your scene an additive layer on top of the current one. WHen you run the PlayGround scene, you will see your scene also visible there.

Since your scene is layered on the main PlayGround scene, you don't need to have those basic components such as camera, light, terrain. All the rendering settings will follow the PlayGround scene settings.

If you want to create your own environment that is not shared with others, you can add a portal as gate in the PlayGround scene that teleports the audience into your world.

#### Automatic additive scene loading in PlayGround
In the future, once your scene has been created, you can add your scene to the automatic loading list, which will load everyone's scene into the main PlayGround scene. A loading script "AdditiveSceneLoad.cs" is provided in the "Assets/Editor" folder, that you can edit and add your loading command into the script. Once you have add a loading command into the script, from the Menu - Custom - Open Scene, you can click and use it to quick load all the scenes (including others) into PlayGround and render them together at the same time.

This method is necessary because you only need to change your own scene and assest. It will not create conflicts when other people are editing and uploading at the same time.

#### Folders
You are expected to create a folder with YOUR NAME, so that all your imported materials (scripts, models, images, audio files) are organized and stored in the proper place. 

#### Plugins
If you need to install some plugins, keep them in the folder "External Plugins", because sometimes plugins can flood the assets folder.