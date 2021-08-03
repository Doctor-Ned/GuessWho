# GuessWho

This simple WPF project represents a single board for a PC version of "Guess Who". The game is set up in the theme of League of Legends.
The champions are split into categories, which are easily configurable from both the code or a .json config file itself.

*GuessWho was not meant to be an open project, hence the messages hard-coded within the code are in **Polish**, which is my native language.*

## Champion preparation

Due to the amount of League of Legends champions being dynamic, all of the icons come from [Riot's Data Dragon](https://developer.riotgames.com/docs/lol).

The icons used are located in **$version/img/champion** subdirectory.

Currently used Data Dragon version:
```
11.15.1
```

Updating data includes overwriting the version above and refilling the **GuessWho/Champions** directory with current icons. The images must be added to the VS project as Resources.

After importing the images, the *GuessWho/Model/Champion.cs* enum must be updated with each new champion. The enum values must be named exactly like the files. The *GuessWho/Model/ChampionProvider.cs* contains a Dictionary mapping *Champion* enum values to user-friendly champion names - by default these will be the same, but for some champions they're not (like "Chogath" -> "Cho'Gath" or "MonkeyKing" -> "Wukong"), so the names can be replaced.

## Third-party tools

The project utilizes the following third-party tools all of which are licensed under the MIT license:

- [**Material Design In XAML Toolkit**](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) for an elegant UI
- [**Json.NET**](https://github.com/JamesNK/Newtonsoft.Json) for config serialization
- [**VirtualizingWrapPanel**](https://github.com/sbaeumlisberger/VirtualizingWrapPanel) for a nice presentation of champions
- **NedMaterialMVVM** (my own tool... that I really need to rework) for basic MVVM functionality