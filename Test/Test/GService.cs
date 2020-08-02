using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Linq;

namespace Test
{
    static class GService
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Test";
        static readonly string SpreadsheetId;
        static readonly SheetsService Service;

        public static void CreateSheet(string sheetName)
        {
            if (Service
                .Spreadsheets
                .Get(SpreadsheetId)
                .Execute()
                .Sheets
                .Select(x => x.Properties.Title)
                .Contains(sheetName))
                return;
            var addSheetRequest = new AddSheetRequest();
            addSheetRequest.Properties = new SheetProperties();
            addSheetRequest.Properties.Title = sheetName;
            BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            batchUpdateSpreadsheetRequest.Requests = new List<Request>();
            batchUpdateSpreadsheetRequest.Requests.Add(new Request { AddSheet = addSheetRequest });
            var batchUpdateRequest = Service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, SpreadsheetId);
            batchUpdateRequest.Execute();
        }

        static string ToLetter(int colIndex)
        {
            int div = colIndex;
            string colLetter = string.Empty;
            while (div > 0)
            {
                int mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (div - mod) / 26;
            }
            return colLetter;
        }

        public static void Update(List<IList<object>> data, string sheetName, int shiftRow = 0, int shiftColumn = 0)
        {
            var range = $"{sheetName}!{ToLetter(shiftColumn + 1)}{shiftRow + 1}:{ToLetter(data.First().Count() + shiftColumn)}{data.Count() + shiftRow}";
            var valueRange = new ValueRange();
            valueRange.Values = data;
            var updateRequest = Service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            updateRequest.Execute();
        }

        static GService()
        {
            GoogleCredential credential;

            using (var stream =
                new FileStream("../../../credits.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            SpreadsheetId = ConfigurationManager.AppSettings["sheetId"];
        }
    }
}
