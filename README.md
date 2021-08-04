# GuessWho

This simple WPF project represents a single board for a PC version of "Guess Who?". The game is set up in the theme of League of Legends.
The champions are split into categories, which are easily configurable from both the code or a .json config file itself.

*GuessWho was not meant to be an open project, hence the messages hard-coded within the code are in **Polish**, which is my native language. As stated in the TODO section below, I am looking forward to changing this!*

## Credits

Credits for the app idea go to **Dr Hodo** from [YouTube](https://www.youtube.com/channel/UCSBeTmftfh52x7G2eMu_dRA), for whom the project was eventually established.

The 'Guess Who?' game is actually the main point in one of his videos: **video is not released yet**.

## Champion preparation

Due to the amount of League of Legends champions being dynamic, all of the icons come from [Riot's Data Dragon](https://developer.riotgames.com/docs/lol).

The icons used are located in **$(version)/img/champion** subdirectory.

Currently used Data Dragon version:
```
11.15.1
```

Updating data includes overwriting the version above and refilling the **GuessWho/Champions** directory with current icons. The images must be added to the VS project as Resources.

After importing the images, the *GuessWho/Model/Champion.cs* enum must be updated with each new champion. The enum values must be named exactly like the files. The *GuessWho/Model/ChampionProvider.cs* contains a Dictionary mapping *Champion* enum values to user-friendly champion names - by default these will be the same, but for some champions they're not (like "Chogath" -> "Cho'Gath" or "MonkeyKing" -> "Wukong"), so the names can be replaced.

## TODO

*It is worth noting that some of the changes below are massive QOL improvements that could remove some of the inconveniences from above*

- Add a program/script to automatically download the Data Dragon, update the images and extract champion information (name, tags, maybe something else) from the jsons
- Add localization, so that the app can easily be translated to english and other languages
- Change the categories to an enum instead of string names to support localization, too
- Maybe more configuration - that could include switching dark mode and changing color palletes

## Third-party tools

The project utilizes the following third-party tools all of which are licensed under the MIT license:

- [**Material Design In XAML Toolkit**](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) for an elegant UI
- [**Json.NET**](https://github.com/JamesNK/Newtonsoft.Json) for config serialization
- [**VirtualizingWrapPanel**](https://github.com/sbaeumlisberger/VirtualizingWrapPanel) for a nice presentation of champions
- **NedMaterialMVVM** (my own package) for basic MVVM functionality