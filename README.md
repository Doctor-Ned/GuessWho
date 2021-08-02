# GuessWho

This simple WPF project represents a single board for a PC version of "Guess Who". The game is set up in the theme of League of Legends.

## Champion preparation

Due to the amount of League of Legends champions being dynamic, all of the icons come from [Riot's Data Dragon](https://developer.riotgames.com/docs/lol).

The icons used are located in **$version/img/champion** subdirectory.

Currently used Data Dragon version:
```
11.15.1
```

Updating data includes overwriting the version above and refilling the **GuessWho/Champions** directory with current icons. The images must be added to the VS project as Resources.

After importing the images, the *GuessWho/Model/Champion.cs* enum must be updated with each new champion. The enum values must be named exactly like the files. The *GuessWho/Model/ChampionProvider.cs* contains a Dictionary mapping *Champion* enum values to user-friendly champion names - by default these will be the same, but for some champions they're not, so the names can be replaced.

The *ChampionProvider* includes a *Validate* method that should be called once at the application's startup to initialize champion names and make sure that all champion icons are accessible.
