# WorkdayCalendar
Contains code for solving the WorkdayCalendar task.

### Features
- Reads text from local file or from web adress
- Parses text and prints word statistics
- Shows frequencies of words
- Shows longest words
- Supports doing analyzis on multiple texts in parallell

### Platforms
.NET 6

### License
Free for commercial use under the MIT License 2.0.

### Usage
```csharp
> WorkdayCalendar.exe interactive
```

```csharp
> WorkdayCalendar.exe calc --startDate="2022-10-10 07:30" --increment=3
```

```
  --startDate       Required. Specify the date to start calculating from. Should be in format yyyy-MM-dd HH:mm
  --increment	    Required. The steps to calculate from the start date. Can be positive or negative and may contain decimals.

```