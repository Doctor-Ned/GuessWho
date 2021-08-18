# GuessWho

This simple WPF project represents a single board for a PC version of "Guess Who?". The game is set up in the theme of League of Legends.
The champions are split into categories, which are easily configurable from both the code or a .json config file itself.
The app itself supports all of the languages that are supported by Riot in League of Legends - the champion data and categories are translated according to Riot's Data Dragon, and you can manually add UI translations if you want.

Currently the app is fully translated into:
- English
- Polish

The other languages that are not mentioned above are only partially supported - most of the UI for them remains English, but any data provided by Riot's Data Dragon (League related) are entirely translated.


## Credits

Credits for the app idea go to **Dr Hodo** from [YouTube](https://www.youtube.com/channel/UCSBeTmftfh52x7G2eMu_dRA), for whom the project was eventually established.

The 'Guess Who?' game is actually the main point in one of his videos: **video is not released yet**.

## Champion preparation

Due to the amount of League of Legends champions being dynamic, all of the icons and localization data come from [Riot's Data Dragon](https://developer.riotgames.com/docs/lol).

To update the localization/champion data, currently all you have to do is build and run the GuessWhoDataManager executable.
By modifying the resource files in the GuessWhoDataManager project, you can add translations for any of the supported languages.
I'm open for new translations!

## TODO

- Maybe more configuration - that could include switching dark mode and changing color palletes
- ToolTip for hovering over categories, which could show the amount of champions of that category shown
- Add category icons
- Add the ability to chose your own character and have its icon and categories accessible all the time

## Third-party tools

The project utilizes the following third-party tools all of which are licensed under the MIT license:

- [**Material Design In XAML Toolkit**](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) [MIT] for an elegant UI
- [**Json.NET**](https://github.com/JamesNK/Newtonsoft.Json) [MIT] for config serialization
- [**VirtualizingWrapPanel**](https://github.com/sbaeumlisberger/VirtualizingWrapPanel) [MIT] for a nice presentation of champions
- [**WPFLocalizationExtension**](https://github.com/XAMLMarkupExtensions/WPFLocalizationExtension/) [MS-PL] for support of different locales
- **NedMaterialMVVM** (my own package) for basic MVVM functionality