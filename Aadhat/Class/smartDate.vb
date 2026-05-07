Module DateHelper


    Private Function ClampToFinancialYear(d As Date, YearStart As Date, YearEnd As Date) As Date
        If d < YearStart Then Return YearStart
        If d > YearEnd Then Return YearEnd
        Return d
    End Function

    Private Function GetSafeToday(YearStart As Date, YearEnd As Date) As Date
        Dim today As Date = Date.Today
        If today >= YearStart AndAlso today <= YearEnd Then
            Return today
        Else
            Return ClampToFinancialYear(today, YearStart, YearEnd)
        End If
    End Function

    Public Function SmartDate(input As String, Optional isToDate As Boolean = False, Optional fieldCount As Integer = 1) As String

        Dim YearStart As Date = FinYearStart
        Dim YearEnd As Date = FinYearEnd

        ' ✅ Blank input → Today logic
        If input Is Nothing OrElse input.Trim() = "" Then
            Return GetSafeToday(YearStart, YearEnd).ToString("dd-MM-yyyy")
        End If

        input = input.Trim().ToLower().Replace(",", "-").Replace(".", "-").Replace("/", "-")

        Dim match = System.Text.RegularExpressions.Regex.Match(input, "^(\d{1,2})([a-z]{2,})$")
        If match.Success Then
            input = match.Groups(1).Value & "-" & match.Groups(2).Value
        End If

        Dim day As Integer = 1
        Dim month As Integer = 0
        Dim year As Integer = 0

        Dim monthMap As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase) From {
            {"jan", 1}, {"january", 1}, {"ja", 1}, {"j", 1},
            {"feb", 2}, {"february", 2}, {"fe", 2}, {"f", 2},
            {"mar", 3}, {"march", 3}, {"ma", 3}, {"m", 3},
            {"apr", 4}, {"april", 4}, {"ap", 4},
            {"may", 5},
            {"jun", 6}, {"june", 6}, {"ju", 6},
            {"jul", 7}, {"july", 7},
            {"aug", 8}, {"august", 8}, {"au", 8},
            {"sep", 9}, {"sept", 9}, {"september", 9}, {"s", 9}, {"se", 9},
            {"oct", 10}, {"october", 10}, {"o", 10}, {"oc", 10},
            {"nov", 11}, {"november", 11}, {"n", 11}, {"no", 11},
            {"dec", 12}, {"december", 12}, {"d", 12}, {"de", 12}
        }

        ' 🔹 Single day input
        If IsNumeric(input) AndAlso input.Length <= 2 Then
            Try
                Dim d As New DateTime(Date.Today.Year, Date.Today.Month, Val(input))
                d = ClampToFinancialYear(d, YearStart, YearEnd)
                Return d.ToString("dd-MM-yyyy")
            Catch
                Return GetSafeToday(YearStart, YearEnd).ToString("dd-MM-yyyy")
            End Try
        End If

        ' 🔹 Month numeric input
        If IsNumeric(input) AndAlso Val(input) >= 1 AndAlso Val(input) <= 12 Then
            Dim m As Integer = Val(input)
            Dim y As Integer

            Try
                Dim d1 As New DateTime(YearStart.Year, m, 1)
                If d1 >= YearStart AndAlso d1 <= YearEnd Then
                    y = YearStart.Year
                Else
                    y = YearEnd.Year
                End If
            Catch
                y = Date.Today.Year
            End Try

            Dim resultDate As Date = If(isToDate AndAlso fieldCount > 1,
                New Date(y, m, Date.DaysInMonth(y, m)),
                New Date(y, m, 1))

            resultDate = ClampToFinancialYear(resultDate, YearStart, YearEnd)
            Return resultDate.ToString("dd-MM-yyyy")
        End If

        ' 🔹 Text month
        If monthMap.ContainsKey(input) Then
            Dim m As Integer = monthMap(input)
            Dim y As Integer

            Try
                Dim d1 As New DateTime(YearStart.Year, m, 1)
                If d1 >= YearStart AndAlso d1 <= YearEnd Then
                    y = YearStart.Year
                Else
                    y = YearEnd.Year
                End If
            Catch
                y = Date.Today.Year
            End Try

            Dim finalDate As Date = If(isToDate AndAlso fieldCount > 1,
                New Date(y, m, Date.DaysInMonth(y, m)),
                New Date(y, m, 1))

            finalDate = ClampToFinancialYear(finalDate, YearStart, YearEnd)
            Return finalDate.ToString("dd-MM-yyyy")
        End If

        ' 🔹 Numeric formats
        If IsNumeric(input) Then
            Try
                If input.Length = 4 Then
                    day = Val(input.Substring(0, 2))
                    month = Val(input.Substring(2, 2))
                ElseIf input.Length = 6 Then
                    day = Val(input.Substring(0, 2))
                    month = Val(input.Substring(2, 2))
                    year = 2000 + Val(input.Substring(4, 2))
                End If
            Catch
                Return GetSafeToday(YearStart, YearEnd).ToString("dd-MM-yyyy")
            End Try
        Else
            Dim parts() As String = input.Split("-"c)
            Try
                If parts.Length = 2 Then
                    If IsNumeric(parts(0)) AndAlso monthMap.ContainsKey(parts(1)) Then
                        day = Val(parts(0))
                        month = monthMap(parts(1))
                    ElseIf monthMap.ContainsKey(parts(0)) AndAlso IsNumeric(parts(1)) Then
                        month = monthMap(parts(0))
                        day = Val(parts(1))
                    ElseIf IsNumeric(parts(0)) AndAlso IsNumeric(parts(1)) Then
                        day = Val(parts(0))
                        month = Val(parts(1))
                    End If
                ElseIf parts.Length >= 3 Then
                    If IsNumeric(parts(0)) Then day = Val(parts(0))
                    If IsNumeric(parts(1)) Then
                        month = Val(parts(1))
                    ElseIf monthMap.ContainsKey(parts(1)) Then
                        month = monthMap(parts(1))
                    End If
                    If IsNumeric(parts(2)) Then
                        year = Val(parts(2))
                        If year < 100 Then year += 2000
                    End If
                End If
            Catch
                Return GetSafeToday(YearStart, YearEnd).ToString("dd-MM-yyyy")
            End Try
        End If

        ' 🔹 Swap fix
        If (day > 31 OrElse month > 12) AndAlso (month <= 31 AndAlso day <= 12) Then
            Dim tmp = day
            day = month
            month = tmp
        End If

        ' 🔹 Year detect
        If year = 0 Then
            Try
                Dim d1 As New DateTime(YearStart.Year, month, day)
                If d1 >= YearStart AndAlso d1 <= YearEnd Then
                    year = YearStart.Year
                Else
                    Dim d2 As New DateTime(YearEnd.Year, month, day)
                    If d2 >= YearStart AndAlso d2 <= YearEnd Then
                        year = YearEnd.Year
                    Else
                        year = YearStart.Year
                    End If
                End If
            Catch
                Return GetSafeToday(YearStart, YearEnd).ToString("dd-MM-yyyy")
            End Try
        End If

        ' 🔹 Final build
        Try
            Dim finalDate As New DateTime(year, month, day)
            finalDate = ClampToFinancialYear(finalDate, YearStart, YearEnd)
            Return finalDate.ToString("dd-MM-yyyy")
        Catch
            Return GetSafeToday(YearStart, YearEnd).ToString("dd-MM-yyyy")
        End Try

    End Function


End Module
