# EmlaCron

## What is it?

A simple command-line application for managing aspects of an Emlalock session.

## Running

Simply download the latest version for your platform (Windows, MacOS or Linux) from the releases section and extract

Options are

* `-u` or `--userId`
    * The Emlalock User ID (Required)

* `-a` or `--apiKey`
    * The API key for the Emlalock user (Required)

* `-r` or `--requirements`
    * Optional parameter. Must be a number. If the user currently has less than this amount of requirements, this many requirements will be added.

    * e.g. if a user has 9 requirements remaining and sets 10, then after this runs, they will have 19 requirements. Will not run if requirements are 0

* `-t` or `--time`
    * Optional parameter. Can either be the time in seconds or using the same short terms (e.g. `W1D2H10M23S15`) as the Emlalock API. If the user currently has less than this amount of time remaining, then this duration will be added to the session.

    * e.g. if a user has 23 hours remaining and sets 1 day, then after this runs, they will have 47 hours remaining. Will not run if there is no time remaning

* `-f` or `--force`
    * Will add time or requirements as above, even if the time or requirements remaining are zero.

## What's the point?

Running it as a once-off doesn't seem to do much. But you can either run it as a [Scheduled Task in Windows](https://www.windowscentral.com/how-create-automated-task-using-task-scheduler-windows-10), or as a cron job in [MacOS](https://medium.com/macoclock/automate-running-a-script-using-crontab-on-macos-88a378e0aeac) or [Linux](https://phoenixnap.com/kb/set-up-cron-job-linux)

Simply set your task to run at an interval of your choice, say every 24 hours, and set it to execute the program with your chosen options.

So set it run daily, ensuring you always have at least 10 requirements....
```
EmlaCron -u WearerUserId -a WearerApiKey -r 10
```

Now, to get released, not only do you need to get 10 requirements, you need to get them within 24 hours! Good luck!

Or do you want to make sure that someone you have locked has a narrow window of opportunity to escape?

Run it every week and set the time to 6 days, 23 hours and 30 minutes. Now they only have a half hour where they can unlock before the clock resets for another week...
```
EmlaCron -u WearerUserId -a WearerApiKey -t D6H23M30 --force
```
