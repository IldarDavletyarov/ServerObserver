# Server Observer
## What is it?
ServerObserver allows you to monitor the status of a remote database on Postgres within Google Sheets in real time.
## How works:
### Connect GoogleSheets:
Pre-create a table and remember its id.
Example: docs.google.com/spreadsheets/d/__1qJodoeJxOdVFUy8SqGApvgtAKygl2Ww4XHKMslQqO1M__ - id in bold.
Go to [link](https://console.developers.google.com/apis/library/sheets.googleapis.com) and enable api on your account.
After setup is complete, a .json file will be downloaded with your account data.
Rename this file to `credits.json` and put it in Test/Test .
In this file copy the mail (the field client_email) and give the rights to edit the table for this mail.
### Connect PostgreSQL:
In the `app.config` configuration file must be entered in the section `Servers` data about servers, where `key` - server address, `value` - server size in GB.
`user`, `password` - PostgreSQL account data.
`delaySeconds` - time between updates in seconds.
`sheetId` - id of the table, which we remembered at the beginning.
