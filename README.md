[![DotNet Build](https://github.com/AntiquePendulum/Dimer/actions/workflows/debug-build.yml/badge.svg)](https://github.com/AntiquePendulum/Dimer/actions/workflows/debug-build.yml)

# Dimer [WIP]
> A Timer for Discord App.

![HowToUse](https://user-images.githubusercontent.com/39259186/128027793-8137f8c4-9733-4efe-a35e-89bdd9a8ae2d.gif)

## Commands
In default, Dimer has 4 commands.

### Add Timer
```
!dimer [time]
!dimer [time] [message]
```

Dimer will Reply `[timer emoji] [timerId]`

Dimer supports the following writing styles
* `10` => 10s
* `10:00` => 10m
* `10:00:00` => 10h
* `1:11:11:11` => 1d 11h 11m 11s
* `180` => 180s

>When the time you set comes, Dimer will send you a notification with [message]!

### Remove Timer [WIP]

```
!dimer-r [timerId]
```

### Edit Message [WIP]

```
!dimer-e [timerId] [message]
```

Change notification message when send by Dimer

Notice: You Can't Change [time]

### Help!

```
!help
!dimer-h
```

## Development
### Requirments
* .NET 5

## License
Under the MIT.