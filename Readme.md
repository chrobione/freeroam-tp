# freeroam-tp

Very basic freeroam teleport resource for GTA Network servers. Currently does not (de)serialize teleport locations - this would be an excercise to the reader. :)

## Usage

To see a list of possible teleport locations: `/tplist`

To teleport to a teleport location: `/tp <name>`

To add a new teleport location (you have to an admin): `/tpset <name>`

To remove an existing teleport location (you have to be an admin): `/tprem <name>`

## Installation

Put the repository as a folder in the `resources` folder and then include it in your config like this:

```XML
<resource src="freeroam-tp" />
```

Or start it via the admin resource via: `/start freeroam-tp`
