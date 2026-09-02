Imports System.Data
Imports System.Data.SQLite
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.Threading
Imports Newtonsoft.Json.Linq

Public Class OtherNameTranslator
    Private isWorking As Boolean = False
    Private cancelRequested As Boolean = False

    Private Class LanguageOption
        Private _displayName As String
        Private _code As String

        Public Sub New(ByVal displayName As String, ByVal code As String)
            _displayName = displayName
            _code = code
        End Sub

        Public ReadOnly Property DisplayName() As String
            Get
                Return _displayName
            End Get
        End Property

        Public ReadOnly Property Code() As String
            Get
                Return _code
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return _code
        End Function
    End Class

    Private Sub OtherNameTranslator_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        LoadLanguages()
        If cbRecordType.Items.Count > 0 AndAlso cbRecordType.SelectedIndex < 0 Then cbRecordType.SelectedIndex = 0
        LoadRecords()
    End Sub

    Private Sub LoadLanguages()
        Dim languages As New List(Of LanguageOption)()
        languages.Add(New LanguageOption("Hindi", "hi"))
        languages.Add(New LanguageOption("English", "en"))
        languages.Add(New LanguageOption("Gujarati", "gu"))
        languages.Add(New LanguageOption("Punjabi", "pa"))
        languages.Add(New LanguageOption("Marathi", "mr"))
        languages.Add(New LanguageOption("Bengali", "bn"))
        languages.Add(New LanguageOption("Tamil", "ta"))
        languages.Add(New LanguageOption("Telugu", "te"))
        languages.Add(New LanguageOption("Kannada", "kn"))
        languages.Add(New LanguageOption("Malayalam", "ml"))
        languages.Add(New LanguageOption("Urdu", "ur"))
        languages.Add(New LanguageOption("Odia", "or"))
        languages.Add(New LanguageOption("Assamese", "as"))
        languages.Add(New LanguageOption("Sanskrit", "sa"))
        languages.Add(New LanguageOption("Nepali", "ne"))
        cbLanguage.DisplayMember = "DisplayName"
        cbLanguage.ValueMember = "Code"
        cbLanguage.DataSource = languages
        If cbLanguage.Items.Count > 0 Then cbLanguage.SelectedIndex = 0
        txtLanguageCode.Text = GetSelectedLanguageCode()
    End Sub

    Private Sub LoadRecords()
        If isWorking Then Exit Sub

        dgNames.Rows.Clear()
        chkSelectAll.Checked = False

        Dim parts As New List(Of String)()
        If IncludeAccounts() Then
            parts.Add("Select 'Account' As RecordType, ID, IfNull(AccountName,'') As SourceName, IfNull(OtherName,'') As OtherName, Cast(IfNull(Tag,0) As Integer) As Tag, Case When Cast(IfNull(Tag,0) As Integer)=0 Then 'Yes' Else 'No' End As PrimaryName From Accounts Where IfNull(AccountName,'')<>''" & BlankOnlyWhere() & PrimaryAccountsWhere())
        End If
        If IncludeItems() Then
            parts.Add("Select 'Item' As RecordType, ID, IfNull(ItemName,'') As SourceName, IfNull(OtherName,'') As OtherName, Null As Tag, '' As PrimaryName From Items Where IfNull(ItemName,'')<>''" & BlankOnlyWhere())
        End If

        If parts.Count = 0 Then
            UpdateSummary()
            Exit Sub
        End If

        Dim sql As String = String.Join(" Union All ", parts.ToArray()) & " Order By RecordType, SourceName"
        Dim dt As DataTable = clsFun.ExecDataTable(sql)
        For Each row As DataRow In dt.Rows
            dgNames.Rows.Add(True,
                             row("RecordType").ToString(),
                             Val(row("ID").ToString()),
                             NormalizeTranslatedText(row("SourceName").ToString()),
                             NormalizeTranslatedText(row("OtherName").ToString()),
                             "",
                             row("PrimaryName").ToString(),
                             row("Tag").ToString(),
                             "Ready")
        Next
        dt.Dispose()

        chkSelectAll.Checked = (dgNames.Rows.Count > 0)
        UpdateSummary()
    End Sub

    Private Function BlankOnlyWhere() As String
        If chkOnlyBlank.Checked Then Return " And IfNull(OtherName,'')=''"
        Return ""
    End Function

    Private Function RecordTypeSelection() As String
        If cbRecordType Is Nothing OrElse cbRecordType.SelectedItem Is Nothing Then Return "Both"
        Return cbRecordType.SelectedItem.ToString()
    End Function

    Private Function IncludeAccounts() As Boolean
        Dim selectedType As String = RecordTypeSelection()
        Return selectedType = "Both" OrElse selectedType = "Accounts"
    End Function

    Private Function IncludeItems() As Boolean
        Dim selectedType As String = RecordTypeSelection()
        Return selectedType = "Both" OrElse selectedType = "Items"
    End Function

    Private Function PrimaryAccountsWhere() As String
        If chkUpdatePrimaryAccountName.Checked Then Return ""
        Return " And Cast(IfNull(Tag,0) As Integer)=1"
    End Function

    Private Sub cbLanguage_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbLanguage.SelectedIndexChanged
        txtLanguageCode.Text = GetSelectedLanguageCode()
    End Sub

    Private Function GetSelectedLanguageCode() As String
        If TypeOf cbLanguage.SelectedItem Is LanguageOption Then
            Return CType(cbLanguage.SelectedItem, LanguageOption).Code
        End If
        If cbLanguage.SelectedValue IsNot Nothing Then Return cbLanguage.SelectedValue.ToString()
        Return txtLanguageCode.Text.Trim()
    End Function

    Private Sub btnLoad_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLoad.Click
        LoadRecords()
    End Sub

    Private Sub cbRecordType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbRecordType.SelectedIndexChanged
        If isWorking Then Exit Sub
        chkUpdatePrimaryAccountName.Enabled = (RecordTypeSelection() <> "Items")
    End Sub

    Private Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkSelectAll.CheckedChanged
        If isWorking Then Exit Sub
        For Each row As DataGridViewRow In dgNames.Rows
            If row.IsNewRow Then Continue For
            row.Cells("colSelect").Value = chkSelectAll.Checked
        Next
        UpdateSummary()
    End Sub

    Private Sub dgNames_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgNames.CurrentCellDirtyStateChanged
        If dgNames.IsCurrentCellDirty Then dgNames.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub dgNames_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgNames.CellValueChanged
        If e.RowIndex >= 0 Then UpdateSummary()
    End Sub

    Private Sub btnTranslateSelected_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTranslateSelected.Click
        If isWorking Then Exit Sub

        Dim languageCode As String = txtLanguageCode.Text.Trim().ToLower()
        If languageCode = "" Then
            MsgBox("Please select target language.", MsgBoxStyle.Critical, "Other Name Translator")
            cbLanguage.Focus()
            Exit Sub
        End If
        If languageCode.Contains("languageoption") Then
            languageCode = GetSelectedLanguageCode()
            txtLanguageCode.Text = languageCode
        End If

        Dim selectedCount As Integer = CountSelectedRows()
        If selectedCount = 0 Then
            MsgBox("Please tick at least one Account or Item.", MsgBoxStyle.Critical, "Other Name Translator")
            Exit Sub
        End If

        Dim confirmText As String = "Preview translations for " & selectedCount.ToString() & " selected record(s)?"
        confirmText &= vbCrLf & "No database changes will be saved until you click Save Changes."
        If MsgBox(confirmText, MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Other Name Translator") <> MsgBoxResult.Yes Then
            Exit Sub
        End If

        cancelRequested = False
        SetWorkingState(True)

        Dim successCount As Integer = 0
        Dim failedCount As Integer = 0
        Dim processedCount As Integer = 0
        pbProgress.Maximum = selectedCount
        pbProgress.Value = 0

        For Each row As DataGridViewRow In dgNames.Rows
            If cancelRequested Then Exit For
            If row.IsNewRow OrElse IsRowChecked(row) = False Then Continue For

            processedCount += 1
            row.Cells("colStatus").Value = "Translating..."
            lblSummary.Text = "Processing " & processedCount.ToString() & " of " & selectedCount.ToString()
            Application.DoEvents()

            Try
                Dim sourceName As String = GetCellText(row, "colSourceName").Trim()
                Dim translatedName As String = NormalizeTranslatedText(ConvertName(sourceName, languageCode))
                If translatedName.Trim() = "" Then Throw New Exception("Blank translation received.")

                row.Cells("colNewOtherName").Value = translatedName
                row.Cells("colStatus").Value = "Ready to Save"
                successCount += 1
            Catch ex As Exception
                row.Cells("colStatus").Value = "Failed: " & ex.Message
                failedCount += 1
            End Try

            If pbProgress.Value < pbProgress.Maximum Then pbProgress.Value += 1
            Thread.Sleep(120)
            Application.DoEvents()
        Next

        SetWorkingState(False)
        UpdateSummary()

        If cancelRequested Then
            MsgBox("Preview cancelled. Ready to save: " & successCount.ToString() & ", Failed: " & failedCount.ToString(), MsgBoxStyle.Information, "Other Name Translator")
        Else
            MsgBox("Preview complete. Ready to save: " & successCount.ToString() & ", Failed: " & failedCount.ToString(), MsgBoxStyle.Information, "Other Name Translator")
        End If
    End Sub

    Private Function ConvertName(ByVal text As String, ByVal languageCode As String) As String
        If chkNameStyle.Checked Then
            Try
                Return NormalizeTranslatedText(NameStyleText(text, languageCode))
            Catch
            End Try
        End If

        Dim translatedText As String = TranslateText(text, languageCode)
        If chkNameStyle.Checked = False AndAlso translatedText.Trim().ToUpper() = text.Trim().ToUpper() AndAlso languageCode <> "en" Then
            Try
                translatedText = NameStyleText(text, languageCode)
            Catch
            End Try
        End If
        Return NormalizeTranslatedText(translatedText)
    End Function

    Private Function TranslateText(ByVal text As String, ByVal languageCode As String) As String
        If text.Trim() = "" Then Return ""
        If languageCode.Trim() = "" OrElse languageCode.ToLower().Contains("languageoption") Then Throw New Exception("Invalid Google language code.")

        Try
            ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
        Catch
        End Try

        Dim url As String = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl=" &
                            Uri.EscapeDataString(languageCode) & "&dt=t&q=" & Uri.EscapeDataString(text)

        Using client As New WebClient()
            client.Encoding = Encoding.UTF8
            client.Headers.Add("User-Agent", "Mozilla/5.0")
            Dim json As String = client.DownloadString(url)
            Dim parsed As JArray = JArray.Parse(json)
            Dim translated As New StringBuilder()
            Dim sentences As JArray = CType(parsed(0), JArray)
            For Each sentence As JToken In sentences
                If sentence IsNot Nothing AndAlso sentence.Type = JTokenType.Array Then
                    translated.Append(sentence(0).ToString())
                End If
            Next
            Return NormalizeTranslatedText(translated.ToString())
        End Using
    End Function

    Private Function NameStyleText(ByVal text As String, ByVal languageCode As String) As String
        If text.Trim() = "" Then Return ""
        If languageCode = "en" Then Return text

        Dim protectedText As String = ApplyHindiNameDictionary(text, languageCode)
        If IsDevanagariNameStyleLanguage(languageCode) AndAlso protectedText <> text AndAlso HasRomanLetters(protectedText) = False Then
            Return NormalizeTranslatedText(protectedText)
        End If

        Dim transliteratedText As String = ""
        Try
            transliteratedText = TransliterateText(protectedText, languageCode)
        Catch
            If protectedText <> text Then Return NormalizeTranslatedText(protectedText)
            Throw
        End Try
        If transliteratedText.Trim() = "" OrElse transliteratedText.Trim().ToUpper() = text.Trim().ToUpper() Then
            Try
                Dim lowerTransliteration As String = TransliterateText(protectedText.ToLower(), languageCode)
                If lowerTransliteration.Trim() <> "" AndAlso lowerTransliteration.Trim().ToUpper() <> text.Trim().ToUpper() Then transliteratedText = lowerTransliteration
            Catch
            End Try
        End If
        If transliteratedText.Trim() = "" OrElse transliteratedText.Trim().ToUpper() = text.Trim().ToUpper() Then
            transliteratedText = TranslateText(text, languageCode)
        End If
        Return NormalizeTranslatedText(transliteratedText)
    End Function

    Private Function TransliterateText(ByVal text As String, ByVal languageCode As String) As String
        If text.Trim() = "" Then Return ""

        Try
            ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
        Catch
        End Try

        Dim inputToolCode As String = GoogleInputToolCode(languageCode)
        If inputToolCode = "" Then Throw New Exception("No name style service for this language.")

        Dim url As String = "https://inputtools.google.com/request?text=" & Uri.EscapeDataString(text) &
                            "&itc=" & Uri.EscapeDataString(inputToolCode) &
                            "&num=1&cp=0&cs=1&ie=utf-8&oe=utf-8"
        Using client As New WebClient()
            client.Encoding = Encoding.UTF8
            client.Headers.Add("User-Agent", "Mozilla/5.0")
            Dim json As String = client.DownloadString(url)
            Dim parsed As JArray = JArray.Parse(json)
            If parsed.Count < 2 OrElse parsed(0).ToString().ToUpper() <> "SUCCESS" Then Throw New Exception("Google name style service failed.")
            Dim data As JArray = CType(parsed(1), JArray)
            If data.Count = 0 Then Throw New Exception("No name style candidate received.")
            Dim item As JArray = CType(data(0), JArray)
            If item.Count < 2 Then Throw New Exception("Invalid name style response.")
            Dim candidates As JArray = CType(item(1), JArray)
            If candidates.Count = 0 Then Throw New Exception("No name style candidate received.")
            Return NormalizeTranslatedText(candidates(0).ToString())
        End Using
    End Function

    Private Function NormalizeTranslatedText(ByVal value As String) As String
        If value Is Nothing Then Return ""
        Dim result As String = value
        result = result.Replace("&amp;", "&")
        result = result.Replace("&AMP;", "&")
        result = result.Replace("＆", "&")
        Return result
    End Function

    Private Function GoogleInputToolCode(ByVal languageCode As String) As String
        Select Case languageCode.ToLower()
            Case "hi"
                Return "hi-t-i0-und"
            Case "gu"
                Return "gu-t-i0-und"
            Case "pa"
                Return "pa-t-i0-und"
            Case "mr"
                Return "mr-t-i0-und"
            Case "bn"
                Return "bn-t-i0-und"
            Case "ta"
                Return "ta-t-i0-und"
            Case "te"
                Return "te-t-i0-und"
            Case "kn"
                Return "kn-t-i0-und"
            Case "ml"
                Return "ml-t-i0-und"
            Case "ur"
                Return "ur-t-i0-und"
            Case "ne"
                Return "ne-t-i0-und"
            Case "sa"
                Return "hi-t-i0-und"
        End Select
        Return ""
    End Function

    Private Function ApplyHindiNameDictionary(ByVal text As String, ByVal languageCode As String) As String
        If IsDevanagariNameStyleLanguage(languageCode) = False Then Return text

        Dim result As String = text
        result = ReplaceWord(result, "a/c", "अकाउंट")
        result = ReplaceWord(result, "ac", "अकाउंट")
        result = ReplaceWord(result, "a.u", "ए यू")
        result = ReplaceWord(result, "a. u", "ए यू")
        result = ReplaceWord(result, "a u", "ए यू")
        result = ReplaceWord(result, "i.u", "आई यू")
        result = ReplaceWord(result, "i. u", "आई यू")
        result = ReplaceWord(result, "i u", "आई यू")
        result = ReplaceWord(result, "commission", "कमीशन")
        result = ReplaceWord(result, "commision", "कमीशन")
        result = ReplaceWord(result, "apple", "एप्पल")
        result = ReplaceWord(result, "kela", "केला")
        result = ReplaceWord(result, "aalu", "आलू")
        result = ReplaceWord(result, "alu", "आलू")
        result = ReplaceWord(result, "aloo", "आलू")
        result = ReplaceWord(result, "banana", "बनाना")
        result = ReplaceWord(result, "mango", "मैंगो")
        result = ReplaceWord(result, "orange", "ऑरेंज")
        result = ReplaceWord(result, "grapes", "ग्रेप्स")
        result = ReplaceWord(result, "papaya", "पपाया")
        result = ReplaceWord(result, "chiku", "चीकू")
        result = ReplaceWord(result, "chikoo", "चीकू")
        result = ReplaceWord(result, "guava", "गुआवा")
        result = ReplaceWord(result, "account", "अकाउंट")
        result = ReplaceWord(result, "and", "एंड")
        result = ReplaceWord(result, "on", "ऑन")
        result = ReplaceWord(result, "me", "में")
        result = ReplaceWord(result, "company", "कंपनी")
        result = ReplaceWord(result, "co", "कंपनी")
        result = ReplaceWord(result, "babu", "बाबू")
        result = ReplaceWord(result, "ismile", "इस्माइल")
        result = ReplaceWord(result, "ismail", "इस्माइल")
        result = ReplaceWord(result, "smile", "स्माइल")
        result = ReplaceWord(result, "salary", "सैलरी")
        result = ReplaceWord(result, "sales", "सेल्स")
        result = ReplaceWord(result, "sale", "सेल")
        result = ReplaceWord(result, "purchase", "परचेज")
        result = ReplaceWord(result, "cash", "कैश")
        result = ReplaceWord(result, "bank", "बैंक")
        result = ReplaceWord(result, "small", "स्मॉल")
        result = ReplaceWord(result, "finance", "फाइनेंस")
        result = ReplaceWord(result, "financie", "फाइनेंस")
        result = ReplaceWord(result, "fiancie", "फाइनेंस")
        result = ReplaceWord(result, "finence", "फाइनेंस")
        result = ReplaceWord(result, "rent", "रेंट")
        result = ReplaceWord(result, "office", "ऑफिस")
        result = ReplaceWord(result, "staff", "स्टाफ")
        result = ReplaceWord(result, "welfare", "वेलफेयर")
        result = ReplaceWord(result, "expense", "एक्सपेंस")
        result = ReplaceWord(result, "expenses", "एक्सपेंस")
        result = ReplaceWord(result, "bonus", "बोनस")
        result = ReplaceWord(result, "payable", "पेयेबल")
        result = ReplaceWord(result, "vegetable", "वेजिटेबल")
        result = ReplaceWord(result, "water", "वाटर")
        result = ReplaceWord(result, "electricity", "इलेक्ट्रिसिटी")
        result = ReplaceWord(result, "telephone", "टेलीफोन")
        result = ReplaceWord(result, "travelling", "ट्रैवलिंग")
        result = ReplaceWord(result, "traveling", "ट्रैवलिंग")
        result = ReplaceWord(result, "tds", "टीडीएस")
        result = ReplaceWord(result, "vat", "वैट")
        result = ReplaceWord(result, "self", "सेल्फ")
        result = ReplaceWord(result, "sabji", "सब्जी")
        result = ReplaceWord(result, "untalad", "ऊंटलाड")
        result = ReplaceWord(result, "unthalad", "ऊंथलाड")
        result = ReplaceWord(result, "unthad", "ऊंथड")
        result = ReplaceWord(result, "untad", "ऊंटड")
        result = ReplaceWord(result, "unthar", "ऊंथर")
        result = ReplaceWord(result, "au", "ए यू")
        result = ApplyInitialDictionary(result)
        Return result
    End Function

    Private Function ReplaceWord(ByVal text As String, ByVal englishWord As String, ByVal hindiWord As String) As String
        Return Regex.Replace(text, "(?i)(?<![A-Za-z])" & Regex.Escape(englishWord) & "(?![A-Za-z])", hindiWord)
    End Function

    Private Function ApplyInitialDictionary(ByVal text As String) As String
        Dim result As String = text
        result = ReplaceWord(result, "a", "ए")
        result = ReplaceWord(result, "b", "बी")
        result = ReplaceWord(result, "c", "सी")
        result = ReplaceWord(result, "d", "डी")
        result = ReplaceWord(result, "e", "ई")
        result = ReplaceWord(result, "f", "एफ")
        result = ReplaceWord(result, "g", "जी")
        result = ReplaceWord(result, "h", "एच")
        result = ReplaceWord(result, "i", "आई")
        result = ReplaceWord(result, "j", "जे")
        result = ReplaceWord(result, "k", "के")
        result = ReplaceWord(result, "l", "एल")
        result = ReplaceWord(result, "m", "एम")
        result = ReplaceWord(result, "n", "एन")
        result = ReplaceWord(result, "o", "ओ")
        result = ReplaceWord(result, "p", "पी")
        result = ReplaceWord(result, "q", "क्यू")
        result = ReplaceWord(result, "r", "आर")
        result = ReplaceWord(result, "s", "एस")
        result = ReplaceWord(result, "t", "टी")
        result = ReplaceWord(result, "u", "यू")
        result = ReplaceWord(result, "v", "वी")
        result = ReplaceWord(result, "w", "डब्ल्यू")
        result = ReplaceWord(result, "x", "एक्स")
        result = ReplaceWord(result, "y", "वाई")
        result = ReplaceWord(result, "z", "जेड")
        Return result
    End Function

    Private Function IsDevanagariNameStyleLanguage(ByVal languageCode As String) As Boolean
        Select Case languageCode.ToLower()
            Case "hi", "ne", "mr", "sa"
                Return True
        End Select
        Return False
    End Function

    Private Function HasRomanLetters(ByVal text As String) As Boolean
        Return Regex.IsMatch(text, "[A-Za-z]")
    End Function

    Private Sub UpdateNames(ByVal recordType As String, ByVal id As Integer, ByVal otherName As String)
        Dim tableName As String = ""
        If recordType = "Account" Then
            tableName = "Accounts"
        ElseIf recordType = "Item" Then
            tableName = "Items"
        Else
            Throw New Exception("Invalid record type.")
        End If

        Using con As SQLiteConnection = clsFun.GetConnection()
            Dim sql As String = "Update " & tableName & " Set OtherName=@OtherName Where ID=@ID"

            Using cmd As New SQLiteCommand(sql, con)
                cmd.Parameters.AddWithValue("@OtherName", otherName)
                cmd.Parameters.AddWithValue("@ID", id)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub btnUpdateTypedNames_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateTypedNames.Click
        If isWorking Then Exit Sub

        dgNames.EndEdit()

        Dim rows As List(Of DataGridViewRow) = RowsForSave()
        If rows.Count = 0 Then
            MsgBox("Please tick a row or click one row, then enter Translated OtherName.", MsgBoxStyle.Critical, "Other Name Translator")
            Exit Sub
        End If

        If MsgBox("Save " & rows.Count.ToString() & " translated name(s) to OtherName?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Other Name Translator") <> MsgBoxResult.Yes Then Exit Sub

        Dim successCount As Integer = 0
        Dim failedCount As Integer = 0
        For Each row As DataGridViewRow In rows
            Try
                Dim typedName As String = NormalizeTranslatedText(GetCellText(row, "colNewOtherName")).Trim()
                If typedName = "" Then Throw New Exception("Translated OtherName is blank.")
                UpdateNames(GetCellText(row, "colRecordType"), Val(GetCellText(row, "colID")), typedName)
                row.Cells("colNewOtherName").Value = typedName
                row.Cells("colOldOtherName").Value = typedName
                row.Cells("colStatus").Value = "Saved"
                successCount += 1
            Catch ex As Exception
                row.Cells("colStatus").Value = "Failed: " & ex.Message
                failedCount += 1
            End Try
        Next
        UpdateSummary()
        MsgBox("Save complete. Saved: " & successCount.ToString() & ", Failed: " & failedCount.ToString(), MsgBoxStyle.Information, "Other Name Translator")
    End Sub

    Private Function RowsForSave() As List(Of DataGridViewRow)
        Dim rows As New List(Of DataGridViewRow)()
        If CountSelectedRows() = 0 Then
            If dgNames.CurrentRow IsNot Nothing AndAlso dgNames.CurrentRow.IsNewRow = False AndAlso GetCellText(dgNames.CurrentRow, "colNewOtherName").Trim() <> "" Then rows.Add(dgNames.CurrentRow)
            Return rows
        End If

        For Each row As DataGridViewRow In dgNames.Rows
            If row.IsNewRow = False AndAlso IsRowChecked(row) AndAlso GetCellText(row, "colNewOtherName").Trim() <> "" Then rows.Add(row)
        Next
        Return rows
    End Function

    Private Function IsPrimaryAccount(ByVal row As DataGridViewRow) As Boolean
        If GetCellText(row, "colRecordType") <> "Account" Then Return False
        Return Val(GetCellText(row, "colTag")) = 0
    End Function

    Private Function CountSelectedRows() As Integer
        Dim count As Integer = 0
        For Each row As DataGridViewRow In dgNames.Rows
            If row.IsNewRow = False AndAlso IsRowChecked(row) Then count += 1
        Next
        Return count
    End Function

    Private Function IsRowChecked(ByVal row As DataGridViewRow) As Boolean
        Dim checkedValue As Boolean = False
        If row.Cells("colSelect").Value IsNot Nothing Then Boolean.TryParse(row.Cells("colSelect").Value.ToString(), checkedValue)
        Return checkedValue
    End Function

    Private Function GetCellText(ByVal row As DataGridViewRow, ByVal columnName As String) As String
        If row.Cells(columnName).Value Is Nothing Then Return ""
        Return row.Cells(columnName).Value.ToString()
    End Function

    Private Sub UpdateSummary()
        lblSummary.Text = "Records: " & dgNames.Rows.Count.ToString() & "    Selected: " & CountSelectedRows().ToString()
    End Sub

    Private Sub SetWorkingState(ByVal working As Boolean)
        isWorking = working
        btnLoad.Enabled = Not working
        btnTranslateSelected.Enabled = Not working
        btnUpdateTypedNames.Enabled = Not working
        btnCancel.Enabled = working
        cbLanguage.Enabled = Not working
        txtLanguageCode.Enabled = Not working
        cbRecordType.Enabled = Not working
        chkOnlyBlank.Enabled = Not working
        chkNameStyle.Enabled = Not working
        chkUpdatePrimaryAccountName.Enabled = Not working AndAlso RecordTypeSelection() <> "Items"
        dgNames.ReadOnly = working
        If working = False Then
            btnCancel.Enabled = False
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        cancelRequested = True
        btnCancel.Enabled = False
        lblSummary.Text = "Cancelling after current record..."
    End Sub

    Private Sub OtherNameTranslator_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape AndAlso isWorking = False Then Me.Close()
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        If isWorking Then
            cancelRequested = True
            lblSummary.Text = "Cancelling after current record..."
        Else
            Me.Close()
        End If
    End Sub
End Class
