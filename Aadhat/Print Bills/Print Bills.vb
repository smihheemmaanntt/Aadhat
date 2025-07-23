Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Text
Imports System.Threading

Public Class Print_Bills
    Dim id As String
    Dim whatsappSender As New WhatsAppSender()
    Private headerCheckBox As CheckBox = New CheckBox()
    Private WhatsappCheckBox As CheckBox = New CheckBox()
    Private WithEvents bgWorker As New System.ComponentModel.BackgroundWorker
    Private isBackgroundWorkerRunning As Boolean = False
    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
        bgWorker.WorkerSupportsCancellation = True
        AddHandler bgWorker.DoWork, AddressOf bgWorker_DoWork
        AddHandler bgWorker.RunWorkerCompleted, AddressOf bgWorker_RunWorkerCompleted
    End Sub

    Private Sub Print_Bills_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If isBackgroundWorkerRunning Then
            e.Cancel = True
            Me.Hide()
            '  MessageBox.Show("The process is still running. The form will be hidden instead of closed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Top = 0 : Me.Left = 0
        End If
    End Sub
    Private Sub Print_Bills_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If pnlSearch.Visible = True Then
                pnlSearch.Visible = False
            ElseIf pnlWhatsapp.Visible = True Then
                pnlWhatsapp.Visible = False
            Else
                If isBackgroundWorkerRunning Then
                    '  e.Cancel = True
                    Me.Hide()
                    '    MessageBox.Show("The process is still running. The form will be hidden instead of closed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Left = 0 : Me.Top = 0
                Else
                    Me.Close()
                End If

            End If
        End If
    End Sub

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectAll()
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectAll()
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub Print_Bills_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'CheckForIllegalCrossThreadCalls = False
        Me.Top = 0 : Me.Left = 0
        pnlSearch.Visible = False
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        mindate = clsFun.ExecScalarStr("Select max(entrydate) from BillPrints ")
        maxdate = mindate
        If mindate <> "" Then mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy") Else mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        If maxdate <> "" Then MsktoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy") Else MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 12
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 100
        dg1.Columns(1).SortMode = DataGridViewColumnSortMode.Automatic
        dg1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).Name = "Item Name" : dg1.Columns(2).Width = 159
        dg1.Columns(2).SortMode = DataGridViewColumnSortMode.Automatic
        dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(3).Name = "Customer" : dg1.Columns(3).Width = 250
        dg1.Columns(3).SortMode = DataGridViewColumnSortMode.Automatic
        dg1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(4).Name = "Nug" : dg1.Columns(4).Width = 80
        dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(5).Name = "Kg" : dg1.Columns(5).Width = 80
        dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(6).Name = "Rate" : dg1.Columns(6).Width = 80
        dg1.Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(7).Name = "Per" : dg1.Columns(7).Width = 70
        dg1.Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        dg1.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dg1.Columns(8).Name = "Basic" : dg1.Columns(8).Width = 120
        dg1.Columns(8).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(9).Name = "Charges" : dg1.Columns(9).Width = 110
        dg1.Columns(9).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(10).Name = "Total" : dg1.Columns(10).Width = 120
        dg1.Columns(10).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(11).Name = "TransType" : dg1.Columns(11).Width = 90
    End Sub
    Private Sub HeaderCheckBox_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        'Necessary to end the edit mode of the Cell.
        dgAccounts.EndEdit()
        'Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
        For Each row As DataGridViewRow In dgAccounts.Rows
            Dim checkBox As DataGridViewCheckBoxCell = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            checkBox.Value = headerCheckBox.Checked
        Next
    End Sub

    Private Sub WhatsappCheckBox_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        DgWhatsapp.EndEdit()
        'Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
        For Each row As DataGridViewRow In DgWhatsapp.Rows
            Dim checkBox As DataGridViewCheckBoxCell = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            checkBox.Value = WhatsappCheckBox.Checked
        Next
    End Sub
    Private Sub IDGentate4Slips()
        Dim id As String = String.Empty
        Dim checkBox As DataGridViewCheckBoxCell
        For Each row As DataGridViewRow In dgAccounts.Rows
            checkBox = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            If checkBox.Value = True Then
                id = id & row.Cells(1).Value & ","
            End If
        Next
        If id = "" Then
            If ckJoin.Checked = True Then retrive3(id) Else retrive2(id)
            Exit Sub
        Else
            id = id.Remove(id.LastIndexOf(","))
            If ckJoin.Checked = True Then retrive3(id) Else retrive2(id)
        End If
    End Sub
    Private Sub IDGentateSlips()
        Dim id As String = String.Empty
        Dim checkBox As DataGridViewCheckBoxCell
        For Each row As DataGridViewRow In dgAccounts.Rows
            checkBox = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            If checkBox.Value = True Then
                id = id & row.Cells(1).Value & ","
            End If
        Next
        If id = "" Then
            'If ckJoin.Checked = True Then SlipsRecord2(id) Else
            SlipsRecord(id)
            Exit Sub
        Else
            id = id.Remove(id.LastIndexOf(","))
            'If ckJoin.Checked = True Then SlipsRecord2(id) Else 
            SlipsRecord(id)
        End If
    End Sub

    Sub DGSearchAccounts()
        dgAccounts.Columns.Clear()
        dgAccounts.ColumnCount = 3
        Dim headerCellLocation As Point = Me.dg1.GetCellDisplayRectangle(0, -1, True).Location
        'Place the Header CheckBox in the Location of the Header Cell.
        headerCheckBox.Location = New Point(headerCellLocation.X + 8, headerCellLocation.Y + 2)
        headerCheckBox.BackColor = Color.IndianRed
        headerCheckBox.Size = New Size(18, 18)
        AddHandler headerCheckBox.Click, AddressOf HeaderCheckBox_Clicked
        dgAccounts.Controls.Add(headerCheckBox)
        Dim checkBoxColumn As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
        checkBoxColumn.HeaderText = ""
        checkBoxColumn.Width = 30
        checkBoxColumn.Name = "checkBoxColumn"
        dgAccounts.Columns.Insert(0, checkBoxColumn)
        dgAccounts.Columns(0).ReadOnly = False
        AddHandler dgAccounts.CellContentClick, AddressOf dgAccounts_CellClick
        dgAccounts.Columns(1).Name = "ID"
        dgAccounts.Columns(1).Visible = False
        dgAccounts.Columns(2).Name = "Account Name"
        dgAccounts.Columns(2).Width = 250
        dgAccounts.Columns(3).Name = "Area"
        dgAccounts.Columns(3).Width = 100
    End Sub

    Sub RowColumsWhatsapp()
        DgWhatsapp.Columns.Clear() : DgWhatsapp.ColumnCount = 8
        Dim headerCellLocation As Point = Me.dg1.GetCellDisplayRectangle(0, -1, True).Location
        'Place the Header CheckBox in the Location of the Header Cell.
        WhatsappCheckBox.Location = New Point(headerCellLocation.X + 10, headerCellLocation.Y + 2)
        WhatsappCheckBox.BackColor = Color.GhostWhite
        WhatsappCheckBox.Size = New Size(18, 18)
        AddHandler WhatsappCheckBox.Click, AddressOf WhatsappCheckBox_Clicked
        DgWhatsapp.Controls.Add(WhatsappCheckBox)
        Dim checkBoxColumn1 As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
        checkBoxColumn1.HeaderText = "" : checkBoxColumn1.Width = 30
        checkBoxColumn1.Name = "checkBoxColumn"
        DgWhatsapp.Columns.Insert(0, checkBoxColumn1)
        DgWhatsapp.Columns(0).ReadOnly = False
        AddHandler DgWhatsapp.CellContentClick, AddressOf DgWhatsapp_CellClick
        DgWhatsapp.Columns(1).Name = "ID" : DgWhatsapp.Columns(1).Visible = False
        DgWhatsapp.Columns(2).Name = "Account Name" : DgWhatsapp.Columns(2).Width = 150
        DgWhatsapp.Columns(3).Name = "Mobile No" : DgWhatsapp.Columns(3).Width = 100
        DgWhatsapp.Columns(4).Name = "message" : DgWhatsapp.Columns(4).Width = 250
        DgWhatsapp.Columns(5).Name = "Other Name" : DgWhatsapp.Columns(5).Visible = False
        DgWhatsapp.Columns(6).Name = "message2" : DgWhatsapp.Columns(6).Visible = False
        DgWhatsapp.Columns(7).Name = "Status" : DgWhatsapp.Columns(7).Width = 210
        DgWhatsapp.Columns(8).Name = "Path" : DgWhatsapp.Columns(8).Width = 200
    End Sub

    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(10).Value), "0.00")
        Next
        txtRoff.Text = Format(Val(TxtGrandTotal.Text) - Math.Round(Val(Val(txtTotBasic.Text) + Val(txtTotCharge.Text)), 2, MidpointRounding.AwayFromZero), "0.00")
    End Sub

    Private Sub Save()
        Dim AllRecord As Integer = Val(tmpgrid.Rows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim LastCount As Integer = 0
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        Dim count As Integer = 0 : Dim cmd As New SQLite.SQLiteCommand
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For i As Integer = 0 To maxRowCount - 1
            FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
            For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                With tmpgrid.Rows(LastRecord)
                    If .Cells(2).Value <> "" Then
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "','" & MsktoDate.Text & "','" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," &
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " &
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " &
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " &
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " &
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "'," &
                                "'" & .Cells(25).Value & "','" & .Cells(26).Value & "','" & .Cells(27).Value & "','" & .Cells(28).Value & "', " &
                                "'" & .Cells(29).Value & "','" & .Cells(30).Value & "','" & .Cells(31).Value & "','" & .Cells(32).Value & "', " &
                                "'" & .Cells(33).Value & "','" & .Cells(34).Value & "','" & .Cells(35).Value & "','" & .Cells(36).Value & "', " &
                                "'" & .Cells(37).Value & "','" & .Cells(38).Value & "','" & .Cells(39).Value & "','" & .Cells(40).Value & "', " &
                                "'" & .Cells(41).Value & "','" & .Cells(42).Value & "','" & .Cells(43).Value & "','" & .Cells(44).Value & "'," &
                                "'" & .Cells(45).Value & "','" & .Cells(46).Value & "','" & .Cells(47).Value & "','" & .Cells(48).Value & "', " &
                                "'" & .Cells(49).Value & "','" & .Cells(50).Value & "','" & .Cells(51).Value & "','" & .Cells(52).Value & "', " &
                                "'" & .Cells(53).Value & "','" & .Cells(54).Value & "','" & .Cells(55).Value & "','" & .Cells(56).Value & "', " &
                                "'" & .Cells(57).Value & "','" & .Cells(58).Value & "','" & .Cells(59).Value & "','" & .Cells(60).Value & "', " &
                                "'" & .Cells(61).Value & "','" & .Cells(62).Value & "','" & .Cells(63).Value & "','" & .Cells(64).Value & "'," &
                                "'" & .Cells(65).Value & "','" & .Cells(66).Value & "','" & .Cells(67).Value & "','" & .Cells(68).Value & "', " &
                                "'" & .Cells(69).Value & "','" & .Cells(70).Value & "','" & .Cells(71).Value & "','" & .Cells(72).Value & "', " &
                                "'" & .Cells(73).Value & "','" & .Cells(74).Value & "','" & .Cells(75).Value & "','" & .Cells(76).Value & "', " &
                                "'" & .Cells(77).Value & "','" & .Cells(78).Value & "','" & .Cells(79).Value & "','" & .Cells(80).Value & "', " &
                                "'" & .Cells(81).Value & "','" & .Cells(82).Value & "','" & .Cells(83).Value & "','" & .Cells(84).Value & "', " &
                                "'" & .Cells(85).Value & "','" & .Cells(86).Value & "','" & .Cells(87).Value & "','" & .Cells(88).Value & "', " &
                                "'" & .Cells(89).Value & "','" & .Cells(90).Value & "','" & .Cells(91).Value & "','" & .Cells(92).Value & "'," &
                                "'" & .Cells(93).Value & "','" & .Cells(94).Value & "','" & marka & "', '" & .Cells(95).Value & "'," &
                                "'" & .Cells(96).Value & "','" & .Cells(97).Value & "','" & .Cells(98).Value & "', " &
                                "'" & .Cells(99).Value & "','" & .Cells(100).Value & "','" & .Cells(101).Value & "','" & .Cells(102).Value & "'," &
                                "'" & .Cells(103).Value & "','" & .Cells(104).Value & "'"
                    End If
                End With
                LastRecord = Val(LastRecord + 1)
            Next
            Try
                If FastQuery = String.Empty Then Exit Sub
                Sql = "insert into Printing(D1,D2,P1, P2,P3, P4, P5, P6,P7,P8,P9, P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " &
                         " P21, P22,P23, P24, P25, P26,P27,P28,P29,P30,P31,P32,P33,P34,P35,P36,P37,P38,P39,P40, " &
                         " P41, P42,P43, P44, P45, P46,P47,P48,P49, P50,P51,P52,P53,P54,P55,P56,P57,P58,P59,P60, " &
                         "P61, P62,P63, P64, P65, P66,P67,P68,P69, P70,P71,P72,P73,P74,P75,P76,P77,P78,P79,P80,T1,T2, " &
                         "T3,T4,T5,T6,M1,M2,M3,M4,M5,M6,P83,P84,M10,M11,M12,M13,M14,M15,M16,M17,M18,M19,M20) " & FastQuery & ""
                ClsFunPrimary.ExecNonQuery(Sql)
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try
        Next
        pnlWait.Visible = False
    End Sub

    Private Sub ShowAccounts(Optional ByVal condtion As String = "")
        dgAccounts.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        ssql = "Select AccountID, AccountName from billPrints where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " Group by AccountID order by accountName "
        dt = clsFun.ExecDataTable(ssql)
        lblTotalBills.Text = "Total Bills : " & dt.Rows.Count
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                dgAccounts.Rows.Add()
                With dgAccounts.Rows(i)
                    .Cells(1).Value = dt.Rows(i)("AccountId").ToString()
                    .Cells(2).Value = clsFun.ExecScalarStr("Select AccountName From Accounts Where ID='" & Val(dt.Rows(i)("AccountId").ToString()) & "'")
                    .Cells(1).ReadOnly = True : .Cells(2).ReadOnly = True
                End With
            Next i
        End If
        lblbillCount.Text = "Bills : " & dgAccounts.RowCount
        dg1.ClearSelection()
    End Sub
    Private Sub ShowWhatsappContacts(Optional ByVal condtion As String = "")
        DgWhatsapp.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        Dim Language As String = clsFun.ExecScalarStr("Select Language From Controls")
        If Language = "Gujrati" Then
            ssql = "Select AccountID, (Select AccountName From Accounts Where ID=Transaction2.AccountID) as AccountName, ' નગ : '||sum(nug)||', વજન : '|| sum(weight) ||', મૂળ : '|| " &
                "sum(amount)  ||',ચાર્જીસ : '|| sum(Charges) ||', કુલ : '|| sum(TotalAmount) as Msg, " &
                "' નગ : '||sum(nug)||', વજન  : '|| sum(weight) ||'વેચન રકમ : '|| sum(amount)  ||' ખર્ચો : '|| sum(Charges) ||' કુલ રકમ : '|| sum(TotalAmount) as Msg2, " &
                "(Select OtherName From Accounts Where ID=Transaction2.AccountID) as OtherName from Transaction2 " &
                " where AccountID<>7 and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " Group by AccountID order by accountName "
        ElseIf Language = "Punjabi" Then
            ssql = "SELECT AccountID, (SELECT AccountName FROM Accounts WHERE ID = Transaction2.AccountID) AS AccountName, ' Nug : ' || SUM(nug) || ', Weight : ' || SUM(weight) || ', Basic : ' || " &
        "SUM(amount) || ' Charges : ' || SUM(Charges) || ' Total : ' || SUM(TotalAmount) AS Msg, " &
        "' ਨਗ : ' || SUM(nug) || ', ਵਜਨ : ' || SUM(weight) || ',ਬਿਕਰੀ ਰਕਮ : ' || SUM(amount) || ', ਖਰਚੇ : ' || SUM(Charges) || ', ਕੁੱਲ ਰਕਮ : ' || SUM(TotalAmount) AS Msg2, " &
        "(SELECT OtherName FROM Accounts WHERE ID = Transaction2.AccountID) AS OtherName FROM Transaction2 " &
        "WHERE AccountID <> 7 AND EntryDate BETWEEN '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " GROUP BY AccountID ORDER BY AccountName"
        Else
            ssql = "Select AccountID, (Select AccountName From Accounts Where ID=Transaction2.AccountID) as AccountName, ' Nug : '||sum(nug)||', Weight : '|| sum(weight) ||', Basic : '|| " &
          "sum(amount)  ||' Charges : '|| sum(Charges) ||' Total : '|| sum(TotalAmount) as Msg, " &
          "' नग : '||sum(nug)||', वज़न : '|| sum(weight) ||',बिक्री रकम : '|| sum(amount)  ||', ख़र्चे : '|| sum(Charges) ||',कुल रकम : '|| sum(TotalAmount) as Msg2, " &
          "(Select OtherName From Accounts Where ID=Transaction2.AccountID) as OtherName from Transaction2 " &
          " where AccountID<>7 and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " Group by AccountID order by accountName "
        End If

        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                DgWhatsapp.Rows.Add()
                With DgWhatsapp.Rows(i)
                    .Cells(1).Value = dt.Rows(i)("AccountID").ToString()
                    .Cells(2).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(3).Value = clsFun.ExecScalarStr("Select MObile1 From Accounts Where ID='" & Val(dt.Rows(i)("AccountId").ToString()) & "'")
                    .Cells(4).Value = "Dear " & dt.Rows(i)("AccountName").ToString() & "," & vbNewLine & " Todays Sale :" & dt.Rows(i)("Msg").ToString()
                    .Cells(5).Value = dt.Rows(i)("OtherName").ToString()
                    .Cells(6).Value = dt.Rows(i)("OtherName").ToString() & "," & vbNewLine & dt.Rows(i)("Msg2").ToString()
                    .Cells(1).ReadOnly = True : .Cells(2).ReadOnly = True
                    .Cells(0).Value = True
                End With
            Next i
        End If
        DgWhatsapp.ClearSelection()
    End Sub
 
    Private Sub SendWhatsApp()
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
        If Not Directory.Exists(directoryName) Then
            Directory.CreateDirectory(directoryName)
        Else
            Directory.Delete(directoryName, True)
            Directory.CreateDirectory(directoryName) ' Recreate the directory
        End If
        rowColums1()
        UpdateProgressBarVisibility(True)
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim fastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        If cbType.InvokeRequired Then
            cbType.Invoke(Sub() selectedIndex = cbType.SelectedIndex)
        Else
            selectedIndex = cbType.SelectedIndex
        End If
        If selectedIndex = 0 Then
            ClsFunWhatsapp.ExecNonQuery("Delete from SendingData")
        Else
            WABA.ExecNonQuery("Delete from SendingData")
        End If
        ' Filter the rows to check both checkbox and non-empty Cell(3)

        Dim filteredRows As List(Of DataGridViewRow) = DgWhatsapp.Rows.Cast(Of DataGridViewRow)().
            Where(Function(row) row.Cells(0).Value = True AndAlso Not String.IsNullOrEmpty(row.Cells(3).Value.ToString())).ToList()
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(Sub() ProgressBar1.Maximum = filteredRows.Count)
        Else
            ProgressBar1.Maximum = filteredRows.Count
        End If
        For Each row As DataGridViewRow In filteredRows
            With row
                UpdateProgressBar(count)
                If selectedindex = 0 Then
                    If btnRadioEnglish.Checked = True AndAlso RadioPDFMsg.Checked = True Then
                        SlipsRecord(.Cells(1).Value)
                        GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                        PrintSlips()
                        Pdf_Genrate.ExportReport("\Formats\English.rpt")
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                        "'" & .Cells(4).Value & "', '" & GlobalData.PdfPath & "'"
                    ElseIf btnRadioEnglish.Checked = True AndAlso RadioPdfOnly.Checked = True Then
                        SlipsRecord(.Cells(1).Value)
                        GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                        PrintSlips()
                        Pdf_Genrate.ExportReport("\Formats\English.rpt")
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                                       "'', '" & GlobalData.PdfPath & "'"
                    ElseIf btnRadioEnglish.Checked = True AndAlso RadioMsgOnly.Checked = True Then
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                         "'" & .Cells(4).Value & "', ''"
                    ElseIf RadioRegional.Checked = True AndAlso RadioPDFMsg.Checked = True Then
                        SlipsRecord(.Cells(1).Value)
                        GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                        PrintSlips()
                        Pdf_Genrate.ExportReport("\Formats\Regional.rpt")
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(5).Value & "','" & .Cells(3).Value & "', " &
                       "'" & .Cells(6).Value & "', '" & GlobalData.PdfPath & "'"
                    ElseIf RadioRegional.Checked = True AndAlso RadioPdfOnly.Checked = True Then
                        SlipsRecord(.Cells(1).Value)
                        GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                        PrintSlips()
                        Pdf_Genrate.ExportReport("\Formats\Regional.rpt")
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(5).Value & "','" & .Cells(3).Value & "', " &
                                       "'', '" & GlobalData.PdfPath & "'"
                    ElseIf RadioRegional.Checked = True AndAlso RadioMsgOnly.Checked = True Then
                        If .Cells(3).Value <> "" Then
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(5).Value & "','" & .Cells(3).Value & "', " &
                                       "'" & .Cells(6).Value & "', ''"
                        End If
                    End If
                Else
                    If btnRadioEnglish.Checked = True AndAlso RadioPDFMsg.Checked = True Then
                        SlipsRecord(.Cells(1).Value)
                        GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                        PrintSlips()
                        Pdf_Genrate.ExportReport("\Formats\English.rpt")
                        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                        "'" & .Cells(4).Value & "', '" & whatsappSender.FilePath & "'"
                    ElseIf btnRadioEnglish.Checked = True AndAlso RadioPdfOnly.Checked = True Then
                        SlipsRecord(.Cells(1).Value)
                        GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                        PrintSlips()
                        Pdf_Genrate.ExportReport("\Formats\English.rpt")
                        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                                       "'', '" & whatsappSender.FilePath & "'"
                    ElseIf btnRadioEnglish.Checked = True AndAlso RadioMsgOnly.Checked = True Then
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                         "'" & .Cells(4).Value & "', ''"
                    ElseIf RadioRegional.Checked = True AndAlso RadioPDFMsg.Checked = True Then
                        SlipsRecord(.Cells(1).Value)
                        GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                        PrintSlips()
                        Pdf_Genrate.ExportReport("\Formats\Regional.rpt")
                        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(5).Value & "','" & .Cells(3).Value & "', " &
                       "'" & .Cells(6).Value & "', '" & whatsappSender.FilePath & "'"
                    ElseIf RadioRegional.Checked = True AndAlso RadioPdfOnly.Checked = True Then
                        SlipsRecord(.Cells(1).Value)
                        GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                        PrintSlips()
                        Pdf_Genrate.ExportReport("\Formats\Regional.rpt")
                        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(5).Value & "','" & .Cells(3).Value & "', " &
                                       "'', '" & whatsappSender.FilePath & "'"
                    ElseIf RadioRegional.Checked = True AndAlso RadioMsgOnly.Checked = True Then
                        If .Cells(3).Value <> "" Then
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(5).Value & "','" & .Cells(3).Value & "', " &
                                       "'" & .Cells(6).Value & "', ''"
                        End If
                    End If
                End If

            End With
            count += 1
        Next
        Try
            If selectedIndex = 0 Then
                Sql = "insert into SendingData(AccountID,AccountName,MobileNos,Message1,AttachedFilepath) " & fastQuery & ";Update Settings Set MinState='N'"
                ClsFunWhatsapp.ExecNonQuery(Sql)
                MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")
            Else
                Sql = "insert into SendingData(AccountID,AccountName,MobileNos,Message1,AttachedFilepath) " & fastQuery & ";Update Settings Set MinState='N'"
                WABA.ExecNonQuery(Sql)
                MsgBox("Bills Sended to WahSoft", vbInformation, "Sended to WahSoft")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            UpdateProgressBarVisibility(False)
            ClsFunWhatsapp.CloseConnection()
        End Try
        UpdateProgressBarVisibility(False)
    End Sub
    Private Sub PrintSlips()
        Dim AllRecord As Integer = Val(tmpgrid.Rows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim LastCount As Integer = 0
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        Dim count As Integer = 0 : Dim cmd As New SQLite.SQLiteCommand
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For i As Integer = 0 To maxRowCount - 1
            FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
            For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                With tmpgrid.Rows(LastRecord)
                    If .Cells(2).Value <> "" Then
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," &
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " &
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " &
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " &
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " &
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "'," &
                                "'" & .Cells(25).Value & "','" & .Cells(26).Value & "','" & .Cells(27).Value & "','" & .Cells(28).Value & "', " &
                                "'" & .Cells(29).Value & "','" & .Cells(30).Value & "','" & .Cells(31).Value & "','" & .Cells(32).Value & "', " &
                                "'" & .Cells(33).Value & "','" & .Cells(34).Value & "','" & .Cells(35).Value & "','" & .Cells(36).Value & "'," &
                                "'" & .Cells(37).Value & "','" & .Cells(38).Value & "','" & .Cells(39).Value & "','" & .Cells(40).Value & "'," &
                                "'" & .Cells(41).Value & "','" & .Cells(42).Value & "','" & .Cells(43).Value & "','" & .Cells(44).Value & "'," &
                                "'" & .Cells(45).Value & "','" & .Cells(46).Value & "','" & .Cells(47).Value & "','" & .Cells(48).Value & "'," &
                                "'" & .Cells(79).Value & "','" & .Cells(80).Value & "','" & .Cells(81).Value & "','" & .Cells(87).Value & "', " &
                                "'" & .Cells(88).Value & "','" & .Cells(93).Value & "','" & .Cells(94).Value & "','" & marka & "','" & .Cells(95).Value & "', " &
                                "'" & .Cells(96).Value & "','" & .Cells(97).Value & "','" & .Cells(98).Value & "','" & .Cells(99).Value & "','" & .Cells(76).Value & "','" & .Cells(77).Value & "'"
                    End If
                End With
                LastRecord = Val(LastRecord + 1)
            Next
            Try
                If FastQuery = String.Empty Then Exit Sub
                Sql = "insert into Printing(P1, P2,P3, P4, P5, P6,P7,P8,P9, " &
                        " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " &
                        " P21, P22,P23, P24, P25, P26,P27,P28,P29, " &
                        " P30,P31,P32,P33,P34,P35,P36,P37,P38,P39,P40,P41,P42,P43,P44,P45,P46,M3,M5,P79,P80,P81,M1,T10,P83,P84,M10,M11,M12,M13,M14,M15,P76,P77)" & FastQuery & ""
                ClsFunPrimary.ExecNonQuery(Sql)
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try
        Next
        pnlWait.Visible = False
    End Sub
    Private Sub PrintSlips1()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim Marka As String = clsFun.ExecScalarStr("Select Marka from Company")
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        ' clsFun.ExecNonQuery(sql)
        For Each row As DataGridViewRow In tmpgrid.Rows
            Application.DoEvents()
            pb1.Minimum = 0
            pb1.Maximum = tmpgrid.Rows.Count
            With row
                pb1.Value = row.Index
                If .Cells(2).Value <> "" Then
                    sql = sql & "insert into Printing(P1, P2,P3, P4, P5, P6,P7,P8,P9, " &
                        " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " &
                        " P21, P22,P23, P24, P25, P26,P27,P28,P29, " &
                        " P30,P31,P32,P33,P34,P35,P36,P37,P38,P39,P40,P41,P42,P43,P44,P45,P46,M3,M5,P79,P80,P81,M1,T10,M10)" &
                        "  values('" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," &
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " &
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " &
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " &
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " &
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "'," &
                                "'" & .Cells(25).Value & "','" & .Cells(26).Value & "','" & .Cells(27).Value & "','" & .Cells(28).Value & "', " &
                                "'" & .Cells(29).Value & "','" & .Cells(30).Value & "','" & .Cells(31).Value & "','" & .Cells(32).Value & "', " &
                                "'" & .Cells(33).Value & "','" & .Cells(34).Value & "','" & .Cells(35).Value & "','" & .Cells(36).Value & "'," &
                                "'" & .Cells(37).Value & "','" & .Cells(38).Value & "','" & .Cells(39).Value & "','" & .Cells(40).Value & "'," &
                                "'" & .Cells(41).Value & "','" & .Cells(42).Value & "','" & .Cells(43).Value & "','" & .Cells(44).Value & "'," &
                                "'" & .Cells(45).Value & "','" & .Cells(46).Value & "','" & .Cells(47).Value & "','" & .Cells(48).Value & "'," &
                                "'" & .Cells(79).Value & "','" & .Cells(80).Value & "','" & .Cells(81).Value & "','" & .Cells(87).Value & "', " &
                                "'" & .Cells(88).Value & "','" & Marka & "');"
                End If
            End With
        Next
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
            If cmd.ExecuteNonQuery() > 0 Then count = +1
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
        pnlWait.Visible = False
        ClsFunPrimary.CloseConnection()
    End Sub
    Private Sub retrive(id)
        dg1.Rows.Clear()
        Dim dt As New DataTable
        If id <> "" Then
            id = " and t.Accountid in (" & id & ")"
        End If
        'dt = clsFun.ExecDataTable("Select * FROM Vouchers v INNER JOIN Transaction2 t ON v.id = t.VoucherID  INNER JOIN Accounts a ON a.ID = t.AccountID WHERE  v.EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and  t.Transtype not in ('Standard Sale','Challan','On Sale') " & id & "  order by EntryDate,a.AccountName")
        dt = clsFun.ExecDataTable("Select EntryDate, VoucherID,t.TransType as TransType,a.AccountName as AccountName ,I.ItemName as ItemName,Nug,Weight,Rate,Per,Amount,Charges,TotalAmount FROM Transaction2 t INNER JOIN Accounts a ON a.ID = t.AccountID  INNER JOIN Items I ON I.ID =t.ItemID WHERE  t.EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and  t.Transtype not in ('Standard Sale','Challan','On Sale')  " & id & "  order by EntryDate,a.AccountName")
        If dt.Rows.Count > 20 Then dg1.Columns(10).Width = 103 Else dg1.Columns(10).Width = 120
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1

                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(5).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(7).Value = dt.Rows(i)("Per").ToString()
                        .Cells(8).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("Charges").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(11).Value = dt.Rows(i)("TransType").ToString()
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        ButtonControl() : DGSearchAccounts()
        ShowAccounts() : retrive(id)
        rowColums1() : tmpgrid.Rows.Clear()
        If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp()
        ShowWhatsappContacts() : ButtonControl()
        lblvoucherCount.Visible = True : lblbillCount.Visible = True
        lblvoucherCount.Text = "Entries : " & dg1.RowCount

    End Sub




    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            'e.SuppressKeyPress = True
            'SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnShow.Focus()
        End Select
    End Sub

    Private Sub ButtonControl()
        For Each b As Button In Me.Controls.OfType(Of Button)()
            If b.Enabled = True Then
                b.Enabled = False
            Else
                b.Enabled = True
            End If
        Next
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If dg1.RowCount = 0 Then
            MsgBox("There is no record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            If CDate(mskFromDate.Text).ToString("dd-MM-yyyy") <> CDate(MsktoDate.Text).ToString("dd-MM-yyyy") Then
                MessageBox.Show("You can print by one date only.", "Date Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                mskFromDate.Focus()
                Exit Sub
            End If
            ButtonControl()
            IDGentate4Slips() ': retrive2(id)
            'ClsFunPrimary.changeCompany()
            Save()
            ButtonControl()
            If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
            Report_Viewer.printReport("\Bill4.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Sub retrive2(ByVal id As String)
        tmpgrid.Rows.Clear()
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim cnt As Integer = -1
        Dim ssql As String = String.Empty
        Dim cratedr As Decimal = 0
        Dim cratecr As Decimal = 0
        Dim cratetot As Decimal = 0
        Dim crateopbal As String = ""
        Dim billNo As Integer = 0
        If id <> "" Then
            id = " and Accountid in (" & id & ")"
        End If
        '     dt = clsFun.ExecDataTable("Select AccountName,Accountid FROM BillPrints WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & "group by AccountName,Accountid order by AccountName")
        If ckCashBankBills.Checked = True Then
            dt = clsFun.ExecDataTable("Select AccountName,Accountid FROM BillPrints WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & " group by AccountName,Accountid order by AccountName")
        Else
            dt = clsFun.ExecDataTable("Select AccountName,Accountid FROM BillPrints WHERE  Accountid not in(7)  and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & " group by AccountName,Accountid order by AccountName")
        End If
        If dt.Rows.Count = 0 Then Exit Sub
        For i = 0 To dt.Rows.Count - 1
            Application.DoEvents()
            If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
            pnlWait.Visible = True
            pb1.Minimum = 0
            pb1.Maximum = dt.Rows.Count
            pb1.Value = i
            Dim opbal As String = ""
            ''''''''''''''''''''' Opening Balance'''''''''''''''''''''''''''''''''''
            opbal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                     " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID")) & " Order by upper(AccountName) ;"))

            ''''''''''''''''''''closing balance'''''''''''''''''''''''''

            ClBal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                     " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID")) & " Order by upper(AccountName) ;"))

            ''''''Total Crates Show''''''
            acID = Val(dt.Rows(i)("AccountID").ToString())

            '''''''''''''crate Balance
            crateopbal = Val(0)
            ssql = "Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,Qty as QtyIn,'0' as QtyOut from CrateVoucher where CrateType ='Crate In' " & IIf(Val(acID) > 0, "and AccountID=" & Val(acID) & "", "") & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'   union all" &
                " Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,'0' as QtyOut,Qty as QtyIn from CrateVoucher where CrateType ='Crate Out' " & IIf(Val(acID) > 0, "and AccountID=" & Val(acID) & "", "") & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'     "
            Dim cratetmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(acID) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim cratetmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(acID) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim cratetmpamt As String = Val(Val(crateopbal) + Val(cratetmpamtdr)) - Val(cratetmpamtcr)
            If cratetmpamt > 0 Then
                crateopbal = Val(cratetmpamt)
            Else
                crateopbal = -Val(cratetmpamt)
            End If
            TodaysCredit = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and EntryDate = '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"))
            todaysDebit = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and EntryDate = '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"))
            If ClBal < 0 Then
                lastbal = Format(Math.Abs(Val(Val(ClBal + todaysDebit) - TodaysCredit)), "0.00") & " Cr"
            Else
                lastbal = Format(Math.Abs(Val(Val(ClBal - todaysDebit))), "0.00") & " Dr"
            End If

            acID = Val(dt.Rows(i)("AccountID").ToString())
            ''''''Total Crates Show''''''
            Dim U As Integer = 0
            Dim cratebal As String = String.Empty
            Dim CrateQty As String = String.Empty
            Dim CrateName As String = String.Empty
            Dim CQty As String = String.Empty
            Dim SingleCrate As String = String.Empty
            Dim dtcrate As New DataTable
            dtcrate = clsFun.ExecDataTable("Select CrateName,CrateName ||':'||" &
            " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') -" &
            " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) as Reciveable," &
                        " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') -" &
            " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) as DueCrates " &
            " FROM CrateVoucher as CV INNER JOIN Account_AcGrp AS ACG ON CV.AccountID = ACG.ID Where EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(acID) & "' Group by AccountID,CrateID Having DueCrates>0 order by upper(ACG.AccountName);")
            Try
                If dtcrate.Rows.Count > 0 Then
                    For U = 0 To dtcrate.Rows.Count - 1
                        If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
                        cratebal = dtcrate.Rows(U)("Reciveable").ToString()
                        CrateQty = CrateQty & ", " & cratebal
                        CrateName = CrateName & dtcrate.Rows(U)("CrateName").ToString() & vbCrLf
                        CQty = CQty & dtcrate.Rows(U)("DueCrates").ToString() & vbCrLf
                        SingleCrate = Val(SingleCrate) + Val(dtcrate.Rows(U)("DueCrates").ToString())
                    Next
                    CrateQty = CrateQty.Trim().TrimStart(",")

                End If
            Catch ex As Exception

            End Try

            '''''''''''''''''''''''''''''''

            If dt.Rows.Count > 0 And dt.Rows.Count > 1 Then
                Application.DoEvents()
                If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
                '  For i = 0 To dt.Rows.Count - 1
                If i Mod 2 = 0 Then
                    cnt = cnt + 1
                    dt1 = clsFun.ExecDataTable("Select * FROM BillPrints WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountName='" & dt.Rows(i)(0) & "' order by AccountName")
                    If dt1.Rows.Count > 0 Then
                        tmpgrid.Rows.Add()
                        With tmpgrid.Rows(cnt)
                            Application.DoEvents()
                            .Cells(23).Value = Format(Val(dt1.Compute("Sum(Nug)", "")), "0.00")
                            .Cells(24).Value = Format(Val(dt1.Compute("Sum(Weight)", "")), "0.00")
                            .Cells(25).Value = Format(Val(dt1.Compute("Sum(CommAmt)", "")), "0.00")
                            .Cells(26).Value = Format(Val(dt1.Compute("Sum(MAmt)", "")), "0.00")
                            .Cells(27).Value = Format(Val(dt1.Compute("Sum(RdfAmt)", "")), "0.00")
                            .Cells(28).Value = Format(Val(dt1.Compute("Sum(TareAmt)", "")), "0.00")
                            .Cells(29).Value = Format(Val(dt1.Compute("Sum(LabourAmt)", "")), "0.00")
                            .Cells(30).Value = Format(Val(dt1.Compute("Sum(Charges)", "")), "0.00")
                            .Cells(31).Value = Format(Val(dt1.Compute("Sum(Amount)", "")), "0.00")
                            .Cells(32).Value = Format(Val(dt1.Compute("Sum(TotalAmount)", "")), "0.00")
                            .Cells(37).Value = If(Val(opbal) >= 0, Format(Math.Abs(Val(opbal)), "0.00") & " Dr", Format(Math.Abs(Val(opbal)), "0.00") & " Cr")
                            .Cells(38).Value = If(Val(ClBal) >= 0, Format(Math.Abs(Val(ClBal)), "0.00") & " Dr", Format(Math.Abs(Val(ClBal)), "0.00") & " Cr")
                            .Cells(40).Value = crateopbal
                            .Cells(87).Value = lastbal
                            .Cells(89).Value = CrateQty
                            .Cells(81).Value = CrateName
                            .Cells(82).Value = CQty
                            .Cells(85).Value = SingleCrate
                            .Cells(91).Value = clsFun.ExecScalarInt("Select RowID From Transaction2 Where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' Group by AccountID,EntryDate Order by RowID")
                            .Cells(39).Value = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'"))
                            dt2 = clsFun.ExecDataTable("Select  ('Last Receipt Rs. : '|| Vouchers.TotalAmount || ' On : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastReceipt,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC limit 3 ;")
                            ' dt2 = clsFun.ExecDataTable("Select Top 2 'Last Receipt Rs. : ' + cstr(Vouchers.TotalAmount) + ' On : '+Cstr(Vouchers.Entrydate) as lastpayment,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC;")
                            For j = 0 To dt2.Rows.Count - 1
                                .Cells(36).Value = .Cells(36).Value & dt2.Rows(j)("lastReceipt").ToString() & vbCrLf
                            Next
                            dt2.Dispose()
                            For j = 0 To dt1.Rows.Count - 1
                                Application.DoEvents()
                                .Cells(1).Value = Format(dt1.Rows(j)("EntryDate"), "dd-MM-yyyy")
                                .Cells(2).Value = .Cells(2).Value & dt1.Rows(j)("Itemname").ToString() & vbCrLf
                                .Cells(3).Value = dt1.Rows(j)("AccountName").ToString()
                                .Cells(4).Value = .Cells(4).Value & Format(Val(dt1.Rows(j)("nug").ToString()), "0.00") & vbCrLf
                                .Cells(5).Value = .Cells(5).Value & Format(Val(dt1.Rows(j)("Weight").ToString()), "0.00") & vbCrLf
                                .Cells(6).Value = .Cells(6).Value & Format(Val(dt1.Rows(j)("Rate").ToString()), "0.00") & vbCrLf
                                .Cells(7).Value = .Cells(7).Value & dt1.Rows(j)("Per").ToString() & vbCrLf
                                .Cells(8).Value = .Cells(8).Value & Format(Val(dt1.Rows(j)("Amount").ToString()), "0.00") & vbCrLf
                                .Cells(9).Value = .Cells(9).Value & Format(Val(dt1.Rows(j)("Charges").ToString()), "0.00") & vbCrLf
                                .Cells(10).Value = .Cells(10).Value & Format(Val(dt1.Rows(j)("TotalAmount").ToString()), "0.00") & vbCrLf
                                .Cells(11).Value = .Cells(11).Value & Format(Val(dt1.Rows(j)("CommPer").ToString()), "0.00") & vbCrLf
                                .Cells(12).Value = .Cells(12).Value & Format(Val(dt1.Rows(j)("CommAmt").ToString()), "0.00") & vbCrLf
                                .Cells(13).Value = .Cells(13).Value & Format(Val(dt1.Rows(j)("MPer").ToString()), "0.00") & vbCrLf
                                .Cells(14).Value = .Cells(14).Value & Format(Val(dt1.Rows(j)("MAmt").ToString()), "0.00") & vbCrLf
                                .Cells(15).Value = .Cells(15).Value & Format(Val(dt1.Rows(j)("RdfPer").ToString()), "0.00") & vbCrLf
                                .Cells(16).Value = .Cells(16).Value & Format(Val(dt1.Rows(j)("RdfAmt").ToString()), "0.00") & vbCrLf
                                .Cells(17).Value = .Cells(17).Value & Format(Val(dt1.Rows(j)("Tare").ToString()), "0.00") & vbCrLf
                                .Cells(18).Value = .Cells(18).Value & Format(Val(dt1.Rows(j)("TareAmt").ToString()), "0.00") & vbCrLf
                                .Cells(19).Value = .Cells(19).Value & Format(Val(dt1.Rows(j)("Labour").ToString()), "0.00") & vbCrLf
                                .Cells(20).Value = .Cells(20).Value & Format(Val(dt1.Rows(j)("LabourAmt").ToString()), "0.00") & vbCrLf
                                .Cells(93).Value = .Cells(93).Value & Val(dt1.Rows(j)("Lot").ToString()) & vbCrLf
                                .Cells(95).Value = dt1.Rows(j)("MobileNo1").ToString()
                                .Cells(96).Value = dt1.Rows(j)("MobileNo2").ToString()
                                .Cells(97).Value = dt1.Rows(j)("Area").ToString()
                                .Cells(98).Value = dt1.Rows(j)("City").ToString()
                                .Cells(99).Value = dt1.Rows(j)("LFNo").ToString()
                                If dt1.Rows(j)("mainTainCrate").ToString() = "Y" Then
                                    .Cells(21).Value = .Cells(21).Value & dt1.Rows(j)("CrateMarka").ToString() & vbCrLf
                                    .Cells(22).Value = .Cells(22).Value & dt1.Rows(j)("CrateQty").ToString() & vbCrLf
                                End If
                                .Cells(34).Value = .Cells(34).Value & dt1.Rows(j)("OtherName").ToString() & vbCrLf
                                .Cells(35).Value = dt1.Rows(j)("AccountNameOther").ToString()
                            Next
                        End With
                    End If
                Else
                    dt1 = clsFun.ExecDataTable("Select * FROM BillPrints WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountName='" & dt.Rows(i)(0) & "' order by AccountName")

                    If dt1.Rows.Count > 0 Then
                        'tmpgrid.Rows.Clear()
                        With tmpgrid.Rows(cnt)
                            Application.DoEvents()
                            .Cells(63).Value = Val(dt1.Compute("Sum(Nug)", ""))
                            .Cells(64).Value = Format(Val(dt1.Compute("Sum(Weight)", "")), "0.00")
                            .Cells(65).Value = Format(Val(dt1.Compute("Sum(CommAmt)", "")), "0.00")
                            .Cells(66).Value = Format(Val(dt1.Compute("Sum(MAmt)", "")), "0.00")
                            .Cells(67).Value = Format(Val(dt1.Compute("Sum(RdfAmt)", "")), "0.00")
                            .Cells(68).Value = Format(Val(dt1.Compute("Sum(TareAmt)", "")), "0.00")
                            .Cells(69).Value = Format(Val(dt1.Compute("Sum(LabourAmt)", "")), "0.00")
                            .Cells(70).Value = Format(Val(dt1.Compute("Sum(Charges)", "")), "0.00")
                            .Cells(71).Value = Format(Val(dt1.Compute("Sum(Amount)", "")), "0.00")
                            .Cells(72).Value = Format(Val(dt1.Compute("Sum(TotalAmount)", "")), "0.00")
                            .Cells(77).Value = If(Val(opbal) >= 0, Format(Math.Abs(Val(opbal)), "0.00") & " Dr", Format(Math.Abs(Val(opbal)), "0.00") & " Cr")
                            .Cells(78).Value = If(Val(ClBal) >= 0, Format(Math.Abs(Val(ClBal)), "0.00") & " Dr", Format(Math.Abs(Val(ClBal)), "0.00") & " Cr")
                            .Cells(80).Value = crateopbal : .Cells(88).Value = lastbal
                            .Cells(90).Value = CrateQty : .Cells(83).Value = CrateName
                            .Cells(84).Value = CQty : .Cells(86).Value = SingleCrate
                            .Cells(92).Value = clsFun.ExecScalarInt("Select RowID From Transaction2 Where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' Group by AccountID,EntryDate Order by RowID")
                            .Cells(79).Value = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'"))
                            dt2 = clsFun.ExecDataTable("Select  ('Last Receipt Rs. : '|| Vouchers.TotalAmount || ' On : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastReceipt,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC limit 3 ;")
                            For j = 0 To dt2.Rows.Count - 1
                                .Cells(76).Value = .Cells(76).Value & dt2.Rows(j)("lastReceipt").ToString() & vbCrLf
                            Next

                            dt2.Dispose()

                            '''''''''''''''''''''''''''''''''''''''''''''''''''''''

                            For j = 0 To dt1.Rows.Count - 1
                                Application.DoEvents()
                                .Cells(41).Value = Format(dt1.Rows(j)("EntryDate"), "dd-MM-yyyy")
                                .Cells(42).Value = .Cells(42).Value & dt1.Rows(j)("Itemname").ToString() & vbCrLf
                                .Cells(43).Value = dt1.Rows(j)("AccountName").ToString()
                                .Cells(44).Value = .Cells(44).Value & Format(Val(dt1.Rows(j)("nug").ToString()), "0.00") & vbCrLf
                                .Cells(45).Value = .Cells(45).Value & Format(Val(dt1.Rows(j)("Weight").ToString()), "0.00") & vbCrLf
                                .Cells(46).Value = .Cells(46).Value & Format(Val(dt1.Rows(j)("Rate").ToString()), "0.00") & vbCrLf
                                .Cells(47).Value = .Cells(47).Value & dt1.Rows(j)("Per").ToString() & vbCrLf
                                .Cells(48).Value = .Cells(48).Value & Format(Val(dt1.Rows(j)("Amount").ToString()), "0.00") & vbCrLf
                                .Cells(49).Value = .Cells(49).Value & Format(Val(dt1.Rows(j)("Charges").ToString()), "0.00") & vbCrLf
                                .Cells(50).Value = .Cells(50).Value & Format(Val(dt1.Rows(j)("TotalAmount").ToString()), "0.00") & vbCrLf
                                .Cells(51).Value = .Cells(51).Value & Format(Val(dt1.Rows(j)("CommPer").ToString()), "0.00") & vbCrLf
                                .Cells(52).Value = .Cells(52).Value & Format(Val(dt1.Rows(j)("CommAmt").ToString()), "0.00") & vbCrLf
                                .Cells(53).Value = .Cells(53).Value & Format(Val(dt1.Rows(j)("MPer").ToString()), "0.00") & vbCrLf
                                .Cells(54).Value = .Cells(54).Value & Format(Val(dt1.Rows(j)("MAmt").ToString()), "0.00") & vbCrLf
                                .Cells(55).Value = .Cells(55).Value & Format(Val(dt1.Rows(j)("RdfPer").ToString()), "0.00") & vbCrLf
                                .Cells(56).Value = .Cells(56).Value & Format(Val(dt1.Rows(j)("RdfAmt").ToString()), "0.00") & vbCrLf
                                .Cells(57).Value = .Cells(57).Value & Format(Val(dt1.Rows(j)("Tare").ToString()), "0.00") & vbCrLf
                                .Cells(58).Value = .Cells(58).Value & Format(Val(dt1.Rows(j)("TareAmt").ToString()), "0.00") & vbCrLf
                                .Cells(59).Value = .Cells(59).Value & Format(Val(dt1.Rows(j)("Labour").ToString()), "0.00") & vbCrLf
                                .Cells(60).Value = .Cells(60).Value & Format(Val(dt1.Rows(j)("LabourAmt").ToString()), "0.00") & vbCrLf
                                .Cells(94).Value = .Cells(94).Value & Val(dt1.Rows(j)("Lot").ToString()) & vbCrLf
                                .Cells(100).Value = dt1.Rows(j)("MobileNo1").ToString()
                                .Cells(101).Value = dt1.Rows(j)("MobileNo2").ToString()
                                .Cells(102).Value = dt1.Rows(j)("Area").ToString()
                                .Cells(103).Value = dt1.Rows(j)("City").ToString()
                                .Cells(104).Value = dt1.Rows(j)("LFNo").ToString()
                                If dt1.Rows(j)("mainTainCrate").ToString() = "Y" Then
                                    .Cells(61).Value = .Cells(61).Value & dt1.Rows(j)("CrateMarka").ToString() & vbCrLf
                                    .Cells(62).Value = .Cells(62).Value & dt1.Rows(j)("CrateQty").ToString() & vbCrLf
                                End If
                                .Cells(74).Value = .Cells(74).Value & dt1.Rows(j)("OtherName").ToString() & vbCrLf
                                .Cells(75).Value = dt1.Rows(j)("AccountNameOther").ToString()
                            Next
                        End With
                    End If
                End If
                'Next i
            Else
                dt1 = clsFun.ExecDataTable("Select * FROM BillPrints WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' And '" & MsktoDate.Text & "' and AccountName='" & dt.Rows(i)(0) & "' order by AccountName")

                If dt1.Rows.Count > 0 Then
                    'tmpgrid.Rows.Clear()
                    With tmpgrid.Rows(0)
                        Application.DoEvents()
                        .Cells(23).Value = Format(Val(dt1.Compute("Sum(Nug)", "")), "0.00")
                        .Cells(24).Value = Format(Val(dt1.Compute("Sum(Weight)", "")), "0.00")
                        .Cells(25).Value = Format(Val(dt1.Compute("Sum(CommAmt)", "")), "0.00")
                        .Cells(26).Value = Format(Val(dt1.Compute("Sum(MAmt)", "")), "0.00")
                        .Cells(27).Value = Format(Val(dt1.Compute("Sum(RdfAmt)", "")), "0.00")
                        .Cells(28).Value = Format(Val(dt1.Compute("Sum(TareAmt)", "")), "0.00")
                        .Cells(29).Value = Format(Val(dt1.Compute("Sum(LabourAmt)", "")), "0.00")
                        .Cells(30).Value = Format(Val(dt1.Compute("Sum(Charges)", "")), "0.00")
                        .Cells(31).Value = Format(Val(dt1.Compute("Sum(Amount)", "")), "0.00")
                        .Cells(32).Value = Format(Val(dt1.Compute("Sum(TotalAmount)", "")), "0.00")
                        .Cells(37).Value = opbal
                        .Cells(38).Value = ClBal
                        .Cells(40).Value = crateopbal
                        .Cells(87).Value = lastbal
                        .Cells(81).Value = CrateName
                        .Cells(82).Value = CQty
                        .Cells(85).Value = SingleCrate
                        .Cells(39).Value = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'"))
                        dt2 = clsFun.ExecDataTable("Select  ('Last Receipt Rs. : '|| Vouchers.TotalAmount || ' On : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastReceipt,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC limit 3 ;")
                        ' dt2 = clsFun.ExecDataTable("Select Top 2 'Last Receipt Rs. : ' + cstr(Vouchers.TotalAmount) + ' On : '+Cstr(Vouchers.Entrydate) as lastpayment,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC;")
                        For j = 0 To dt2.Rows.Count - 1
                            .Cells(36).Value = .Cells(36).Value & dt2.Rows(j)("lastReceipt").ToString() & vbCrLf

                        Next
                        dt2.Dispose()

                        '''''''''''''''''''''''''''''''''''''''''''''''''''

                        For j = 0 To dt1.Rows.Count - 1
                            Application.DoEvents()
                            .Cells(1).Value = Format(dt1.Rows(j)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = .Cells(2).Value & dt1.Rows(j)("Itemname").ToString() & vbCrLf
                            .Cells(3).Value = dt1.Rows(j)("AccountName").ToString()
                            .Cells(4).Value = .Cells(4).Value & Format(Val(dt1.Rows(j)("nug").ToString()), "0.00") & vbCrLf
                            .Cells(5).Value = .Cells(5).Value & Format(Val(dt1.Rows(j)("Weight").ToString()), "0.00") & vbCrLf
                            .Cells(6).Value = .Cells(6).Value & Format(Val(dt1.Rows(j)("Rate").ToString()), "0.00") & vbCrLf
                            .Cells(7).Value = .Cells(7).Value & dt1.Rows(j)("Per").ToString() & vbCrLf
                            .Cells(8).Value = .Cells(8).Value & Format(Val(dt1.Rows(j)("Amount").ToString()), "0.00") & vbCrLf
                            .Cells(9).Value = .Cells(9).Value & Format(Val(dt1.Rows(j)("Charges").ToString()), "0.00") & vbCrLf
                            .Cells(10).Value = .Cells(10).Value & Format(Val(dt1.Rows(j)("TotalAmount").ToString()), "0.00") & vbCrLf
                            .Cells(11).Value = .Cells(11).Value & Format(Val(dt1.Rows(j)("CommPer").ToString()), "0.00") & vbCrLf
                            .Cells(12).Value = .Cells(12).Value & Format(Val(dt1.Rows(j)("CommAmt").ToString()), "0.00") & vbCrLf
                            .Cells(13).Value = .Cells(13).Value & Format(Val(dt1.Rows(j)("MPer").ToString()), "0.00") & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & Format(Val(dt1.Rows(j)("MAmt").ToString()), "0.00") & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & Format(Val(dt1.Rows(j)("RdfPer").ToString()), "0.00") & vbCrLf
                            .Cells(16).Value = .Cells(16).Value & Format(Val(dt1.Rows(j)("RdfAmt").ToString()), "0.00") & vbCrLf
                            .Cells(17).Value = .Cells(17).Value & Format(Val(dt1.Rows(j)("Tare").ToString()), "0.00") & vbCrLf
                            .Cells(18).Value = .Cells(18).Value & Format(Val(dt1.Rows(j)("TareAmt").ToString()), "0.00") & vbCrLf
                            .Cells(19).Value = .Cells(19).Value & Format(Val(dt1.Rows(j)("Labour").ToString()), "0.00") & vbCrLf
                            .Cells(20).Value = .Cells(20).Value & Format(Val(dt1.Rows(j)("LabourAmt").ToString()), "0.00") & vbCrLf
                            If dt1.Rows(j)("mainTainCrate").ToString() = "Y" Then
                                .Cells(21).Value = .Cells(21).Value & dt1.Rows(j)("CrateMarka").ToString() & vbCrLf
                                .Cells(22).Value = .Cells(22).Value & dt1.Rows(j)("CrateQty").ToString() & vbCrLf
                            End If
                            .Cells(34).Value = .Cells(34).Value & dt1.Rows(j)("OtherName").ToString() & vbCrLf
                            .Cells(35).Value = .Cells(35).Value & dt1.Rows(j)("AccountNameOther").ToString() & vbCrLf
                        Next
                    End With
                End If
            End If
        Next i
        pnlWait.Visible = False
        dt.Clear()
        dt1.Clear()
        dt2.Clear()
    End Sub


    Sub retrive3(ByVal id As String)
        tmpgrid.Rows.Clear()
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim cnt As Integer = -1
        Dim ssql As String = String.Empty
        Dim cratedr As Decimal = 0
        Dim cratecr As Decimal = 0
        Dim cratetot As Decimal = 0
        Dim crateopbal As String = ""
        Dim billNo As Integer = 0
        If id <> "" Then
            id = " and Accountid in (" & id & ")"
        End If
        '     dt = clsFun.ExecDataTable("Select AccountName,Accountid FROM BillPrints2 WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & "group by AccountName,Accountid order by AccountName")
        If ckCashBankBills.Checked = True Then
            dt = clsFun.ExecDataTable("Select AccountName,Accountid FROM BillPrints2 WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & " group by AccountName,Accountid order by AccountName")
        Else
            dt = clsFun.ExecDataTable("Select AccountName,Accountid FROM BillPrints2 WHERE  Accountid not in(7)  and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & " group by AccountName,Accountid order by AccountName")
        End If
        If dt.Rows.Count = 0 Then Exit Sub
        For i = 0 To dt.Rows.Count - 1
            Application.DoEvents()
            If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
            pnlWait.Visible = True
            pb1.Minimum = 0
            pb1.Maximum = dt.Rows.Count
            pb1.Value = i
            Dim opbal As String = ""
            ''''''''''''''''''''' Opening Balance'''''''''''''''''''''''''''''''''''
            opbal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                     " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID")) & " Order by upper(AccountName) ;"))

            ''''''''''''''''''''closing balance'''''''''''''''''''''''''

            ClBal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                     " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID")) & " Order by upper(AccountName) ;"))

            TodaysCredit = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and EntryDate = '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"))
            todaysDebit = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and EntryDate = '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"))
            If ClBal < 0 Then
                lastbal = Format(Math.Abs(Val(Val(ClBal + todaysDebit) - TodaysCredit)), "0.00") & " Cr"
            Else
                lastbal = Format(Math.Abs(Val(Val(ClBal - todaysDebit))), "0.00") & " Dr"
            End If

            ''''''Total Crates Show''''''
            acID = Val(dt.Rows(i)("AccountID").ToString())

            '''''''''''''crate Balance

            'opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID= " & Val(txtAccountID.Text) & "")
            crateopbal = Val(0)
            ssql = "Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,Qty as QtyIn,'0' as QtyOut from CrateVoucher where CrateType ='Crate In' " & IIf(Val(acID) > 0, "and AccountID=" & Val(acID) & "", "") & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'   union all" &
                " Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,'0' as QtyOut,Qty as QtyIn from CrateVoucher where CrateType ='Crate Out' " & IIf(Val(acID) > 0, "and AccountID=" & Val(acID) & "", "") & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'     "
            Dim cratetmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(acID) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim cratetmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(acID) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim cratetmpamt As String = Val(Val(crateopbal) + Val(cratetmpamtdr)) - Val(cratetmpamtcr)
            If cratetmpamt > 0 Then
                crateopbal = Val(cratetmpamt)
            Else
                crateopbal = -Val(cratetmpamt)
            End If

            acID = Val(dt.Rows(i)("AccountID").ToString())
            ''''''Total Crates Show''''''
            Dim U As Integer = 0
            Dim cratebal As String = String.Empty
            Dim CrateQty As String = String.Empty
            Dim CrateName As String = String.Empty
            Dim CQty As String = String.Empty
            Dim SingleCrate As String = String.Empty
            Dim dtcrate As New DataTable
            dtcrate = clsFun.ExecDataTable("Select CrateName,CrateName ||':'||" &
            " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') -" &
            " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) as Reciveable," &
                        " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') -" &
            " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) as DueCrates " &
            " FROM CrateVoucher as CV INNER JOIN Account_AcGrp AS ACG ON CV.AccountID = ACG.ID Where EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(acID) & "' Group by AccountID,CrateID Having DueCrates>0 order by upper(ACG.AccountName);")
            Try
                If dtcrate.Rows.Count > 0 Then
                    For U = 0 To dtcrate.Rows.Count - 1
                        If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
                        cratebal = dtcrate.Rows(U)("Reciveable").ToString()
                        CrateQty = CrateQty & ", " & cratebal
                        CrateName = CrateName & dtcrate.Rows(U)("CrateName").ToString() & vbCrLf
                        CQty = CQty & dtcrate.Rows(U)("DueCrates").ToString() & vbCrLf
                        SingleCrate = Val(SingleCrate) + Val(dtcrate.Rows(U)("DueCrates").ToString())
                    Next
                    CrateQty = CrateQty.Trim().TrimStart(",")

                End If
            Catch ex As Exception

            End Try



            '''''''''''''''''''''''''''''''

            If dt.Rows.Count > 0 And dt.Rows.Count > 1 Then
                Application.DoEvents()
                If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
                '  For i = 0 To dt.Rows.Count - 1
                If i Mod 2 = 0 Then
                    cnt = cnt + 1
                    dt1 = clsFun.ExecDataTable("Select * FROM BillPrints2 WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountName='" & dt.Rows(i)(0) & "' order by AccountName")
                    If dt1.Rows.Count > 0 Then
                        tmpgrid.Rows.Add()
                        With tmpgrid.Rows(cnt)
                            Application.DoEvents()
                            .Cells(23).Value = Format(Val(dt1.Compute("Sum(Nug)", "")), "0.00")
                            .Cells(24).Value = Format(Val(dt1.Compute("Sum(Weight)", "")), "0.00")
                            .Cells(25).Value = Format(Val(dt1.Compute("Sum(CommAmt)", "")), "0.00")
                            .Cells(26).Value = Format(Val(dt1.Compute("Sum(MAmt)", "")), "0.00")
                            .Cells(27).Value = Format(Val(dt1.Compute("Sum(RdfAmt)", "")), "0.00")
                            .Cells(28).Value = Format(Val(dt1.Compute("Sum(TareAmt)", "")), "0.00")
                            .Cells(29).Value = Format(Val(dt1.Compute("Sum(LabourAmt)", "")), "0.00")
                            .Cells(30).Value = Format(Val(dt1.Compute("Sum(Charges)", "")), "0.00")
                            .Cells(31).Value = Format(Val(dt1.Compute("Sum(Amount)", "")), "0.00")
                            .Cells(32).Value = Format(Val(dt1.Compute("Sum(TotalAmount)", "")), "0.00")
                            .Cells(37).Value = If(Val(opbal) >= 0, Format(Math.Abs(Val(opbal)), "0.00") & " Dr", Format(Math.Abs(Val(opbal)), "0.00") & " Cr")
                            .Cells(38).Value = If(Val(ClBal) >= 0, Format(Math.Abs(Val(ClBal)), "0.00") & " Dr", Format(Math.Abs(Val(ClBal)), "0.00") & " Cr")
                            .Cells(40).Value = crateopbal
                            .Cells(87).Value = lastbal
                            .Cells(89).Value = CrateQty
                            .Cells(81).Value = CrateName
                            .Cells(82).Value = CQty
                            .Cells(85).Value = SingleCrate

                            .Cells(91).Value = clsFun.ExecScalarInt("Select RowID From Transaction2 Where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' Group by AccountID,EntryDate Order by RowID")
                            .Cells(39).Value = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'"))
                            dt2 = clsFun.ExecDataTable("Select  ('Last Receipt Rs. : '|| Vouchers.BasicAmount || ' On : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastReceipt,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC limit 3 ;")
                            ' dt2 = clsFun.ExecDataTable("Select Top 2 'Last Receipt Rs. : ' + cstr(Vouchers.TotalAmount) + ' On : '+Cstr(Vouchers.Entrydate) as lastpayment,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC;")
                            For j = 0 To dt2.Rows.Count - 1
                                .Cells(36).Value = .Cells(36).Value & dt2.Rows(j)("lastReceipt").ToString() & vbCrLf
                            Next
                            dt2.Dispose()


                            '''''''''''''''''''''''''''''''''''''''''''''''''''

                            For j = 0 To dt1.Rows.Count - 1
                                Application.DoEvents()
                                .Cells(1).Value = Format(dt1.Rows(j)("EntryDate"), "dd-MM-yyyy")
                                .Cells(2).Value = .Cells(2).Value & dt1.Rows(j)("Itemname").ToString() & vbCrLf
                                .Cells(3).Value = dt1.Rows(j)("AccountName").ToString()
                                .Cells(4).Value = .Cells(4).Value & Format(Val(dt1.Rows(j)("nug").ToString()), "0.00") & vbCrLf
                                .Cells(5).Value = .Cells(5).Value & Format(Val(dt1.Rows(j)("Weight").ToString()), "0.00") & vbCrLf
                                .Cells(6).Value = .Cells(6).Value & Format(Val(dt1.Rows(j)("Rate").ToString()), "0.00") & vbCrLf
                                .Cells(7).Value = .Cells(7).Value & dt1.Rows(j)("Per").ToString() & vbCrLf
                                .Cells(8).Value = .Cells(8).Value & Format(Val(dt1.Rows(j)("Amount").ToString()), "0.00") & vbCrLf
                                .Cells(9).Value = .Cells(9).Value & Format(Val(dt1.Rows(j)("Charges").ToString()), "0.00") & vbCrLf
                                .Cells(10).Value = .Cells(10).Value & Format(Val(dt1.Rows(j)("TotalAmount").ToString()), "0.00") & vbCrLf
                                .Cells(11).Value = .Cells(11).Value & Format(Val(dt1.Rows(j)("CommPer").ToString()), "0.00") & vbCrLf
                                .Cells(12).Value = .Cells(12).Value & Format(Val(dt1.Rows(j)("CommAmt").ToString()), "0.00") & vbCrLf
                                .Cells(13).Value = .Cells(13).Value & Format(Val(dt1.Rows(j)("MPer").ToString()), "0.00") & vbCrLf
                                .Cells(14).Value = .Cells(14).Value & Format(Val(dt1.Rows(j)("MAmt").ToString()), "0.00") & vbCrLf
                                .Cells(15).Value = .Cells(15).Value & Format(Val(dt1.Rows(j)("RdfPer").ToString()), "0.00") & vbCrLf
                                .Cells(16).Value = .Cells(16).Value & Format(Val(dt1.Rows(j)("RdfAmt").ToString()), "0.00") & vbCrLf
                                .Cells(17).Value = .Cells(17).Value & Format(Val(dt1.Rows(j)("Tare").ToString()), "0.00") & vbCrLf
                                .Cells(18).Value = .Cells(18).Value & Format(Val(dt1.Rows(j)("TareAmt").ToString()), "0.00") & vbCrLf
                                .Cells(19).Value = .Cells(19).Value & Format(Val(dt1.Rows(j)("Labour").ToString()), "0.00") & vbCrLf
                                .Cells(20).Value = .Cells(20).Value & Format(Val(dt1.Rows(j)("LabourAmt").ToString()), "0.00") & vbCrLf
                                If dt1.Rows(j)("mainTainCrate").ToString() = "Y" Then
                                    .Cells(21).Value = .Cells(21).Value & dt1.Rows(j)("CrateMarka").ToString() & vbCrLf
                                    .Cells(22).Value = .Cells(22).Value & dt1.Rows(j)("CrateQty").ToString() & vbCrLf
                                End If

                                .Cells(34).Value = .Cells(34).Value & dt1.Rows(j)("OtherName").ToString() & vbCrLf
                                .Cells(35).Value = .Cells(35).Value & dt1.Rows(j)("AccountNameOther").ToString() & vbCrLf
                            Next
                        End With
                    End If
                Else
                    dt1 = clsFun.ExecDataTable("Select * FROM BillPrints2 WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountName='" & dt.Rows(i)(0) & "' order by AccountName")

                    If dt1.Rows.Count > 0 Then




                        'tmpgrid.Rows.Clear()
                        With tmpgrid.Rows(cnt)
                            Application.DoEvents()
                            .Cells(63).Value = Format(Val(dt1.Compute("Sum(Nug)", "")), "0.00")
                            .Cells(64).Value = Format(Val(dt1.Compute("Sum(Weight)", "")), "0.00")
                            .Cells(65).Value = Format(Val(dt1.Compute("Sum(CommAmt)", "")), "0.00")
                            .Cells(66).Value = Format(Val(dt1.Compute("Sum(MAmt)", "")), "0.00")
                            .Cells(67).Value = Format(Val(dt1.Compute("Sum(RdfAmt)", "")), "0.00")
                            .Cells(68).Value = Format(Val(dt1.Compute("Sum(TareAmt)", "")), "0.00")
                            .Cells(69).Value = Format(Val(dt1.Compute("Sum(LabourAmt)", "")), "0.00")
                            .Cells(70).Value = Format(Val(dt1.Compute("Sum(Charges)", "")), "0.00")
                            .Cells(71).Value = Format(Val(dt1.Compute("Sum(Amount)", "")), "0.00")
                            .Cells(72).Value = Format(Val(dt1.Compute("Sum(TotalAmount)", "")), "0.00")
                            .Cells(77).Value = If(Val(opbal) >= 0, Format(Math.Abs(Val(opbal)), "0.00") & " Dr", Format(Math.Abs(Val(opbal)), "0.00") & " Cr")
                            .Cells(78).Value = If(Val(ClBal) >= 0, Format(Math.Abs(Val(ClBal)), "0.00") & " Dr", Format(Math.Abs(Val(ClBal)), "0.00") & " Cr")
                            .Cells(80).Value = crateopbal
                            .Cells(88).Value = lastbal
                            .Cells(90).Value = CrateQty
                            .Cells(83).Value = CrateName
                            .Cells(84).Value = CQty
                            .Cells(86).Value = SingleCrate
                            .Cells(92).Value = clsFun.ExecScalarInt("Select RowID From Transaction2 Where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' Group by AccountID,EntryDate Order by RowID")
                            .Cells(79).Value = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'"))
                            dt2 = clsFun.ExecDataTable("Select  ('Last Receipt Rs. : '|| Vouchers.TotalAmount || ' On : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastReceipt,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC limit 3 ;")
                            For j = 0 To dt2.Rows.Count - 1
                                .Cells(76).Value = .Cells(76).Value & dt2.Rows(j)("lastReceipt").ToString() & vbCrLf
                            Next

                            dt2.Dispose()

                            '''''''''''''''''''''''''''''''''''''''''''''''''''
                            For j = 0 To dt1.Rows.Count - 1
                                Application.DoEvents()
                                .Cells(41).Value = Format(dt1.Rows(j)("EntryDate"), "dd-MM-yyyy")
                                .Cells(42).Value = .Cells(42).Value & dt1.Rows(j)("Itemname").ToString() & vbCrLf
                                .Cells(43).Value = dt1.Rows(j)("AccountName").ToString()
                                .Cells(44).Value = .Cells(44).Value & Format(Val(dt1.Rows(j)("nug").ToString()), "0.00") & vbCrLf
                                .Cells(45).Value = .Cells(45).Value & Format(Val(dt1.Rows(j)("Weight").ToString()), "0.00") & vbCrLf
                                .Cells(46).Value = .Cells(46).Value & Format(Val(dt1.Rows(j)("Rate").ToString()), "0.00") & vbCrLf
                                .Cells(47).Value = .Cells(47).Value & dt1.Rows(j)("Per").ToString() & vbCrLf
                                .Cells(48).Value = .Cells(48).Value & Format(Val(dt1.Rows(j)("Amount").ToString()), "0.00") & vbCrLf
                                .Cells(49).Value = .Cells(49).Value & Format(Val(dt1.Rows(j)("Charges").ToString()), "0.00") & vbCrLf
                                .Cells(50).Value = .Cells(50).Value & Format(Val(dt1.Rows(j)("TotalAmount").ToString()), "0.00") & vbCrLf
                                .Cells(51).Value = .Cells(51).Value & Format(Val(dt1.Rows(j)("CommPer").ToString()), "0.00") & vbCrLf
                                .Cells(52).Value = .Cells(52).Value & Format(Val(dt1.Rows(j)("CommAmt").ToString()), "0.00") & vbCrLf
                                .Cells(53).Value = .Cells(53).Value & Format(Val(dt1.Rows(j)("MPer").ToString()), "0.00") & vbCrLf
                                .Cells(54).Value = .Cells(54).Value & Format(Val(dt1.Rows(j)("MAmt").ToString()), "0.00") & vbCrLf
                                .Cells(55).Value = .Cells(55).Value & Format(Val(dt1.Rows(j)("RdfPer").ToString()), "0.00") & vbCrLf
                                .Cells(56).Value = .Cells(56).Value & Format(Val(dt1.Rows(j)("RdfAmt").ToString()), "0.00") & vbCrLf
                                .Cells(57).Value = .Cells(57).Value & Format(Val(dt1.Rows(j)("Tare").ToString()), "0.00") & vbCrLf
                                .Cells(58).Value = .Cells(58).Value & Format(Val(dt1.Rows(j)("TareAmt").ToString()), "0.00") & vbCrLf
                                .Cells(59).Value = .Cells(59).Value & Format(Val(dt1.Rows(j)("Labour").ToString()), "0.00") & vbCrLf
                                .Cells(60).Value = .Cells(60).Value & Format(Val(dt1.Rows(j)("LabourAmt").ToString()), "0.00") & vbCrLf
                                If dt1.Rows(j)("mainTainCrate").ToString() = "Y" Then
                                    .Cells(61).Value = .Cells(61).Value & dt1.Rows(j)("CrateMarka").ToString() & vbCrLf
                                    .Cells(62).Value = .Cells(62).Value & dt1.Rows(j)("CrateQty").ToString() & vbCrLf
                                End If
                                .Cells(74).Value = .Cells(74).Value & dt1.Rows(j)("OtherName").ToString() & vbCrLf
                                .Cells(75).Value = .Cells(75).Value & dt1.Rows(j)("AccountNameOther").ToString() & vbCrLf
                            Next
                        End With
                    End If
                End If
                'Next i
            Else
                dt1 = clsFun.ExecDataTable("Select * FROM BillPrints2 WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' And '" & MsktoDate.Text & "' and AccountName='" & dt.Rows(i)(0) & "' order by AccountName")

                If dt1.Rows.Count > 0 Then
                    'tmpgrid.Rows.Clear()
                    With tmpgrid.Rows(0)
                        Application.DoEvents()
                        .Cells(23).Value = Format(Val(dt1.Compute("Sum(Nug)", "")), "0.00")
                        .Cells(24).Value = Format(Val(dt1.Compute("Sum(Weight)", "")), "0.00")
                        .Cells(25).Value = Format(Val(dt1.Compute("Sum(CommAmt)", "")), "0.00")
                        .Cells(26).Value = Format(Val(dt1.Compute("Sum(MAmt)", "")), "0.00")
                        .Cells(27).Value = Format(Val(dt1.Compute("Sum(RdfAmt)", "")), "0.00")
                        .Cells(28).Value = Format(Val(dt1.Compute("Sum(TareAmt)", "")), "0.00")
                        .Cells(29).Value = Format(Val(dt1.Compute("Sum(LabourAmt)", "")), "0.00")
                        .Cells(30).Value = Format(Val(dt1.Compute("Sum(Charges)", "")), "0.00")
                        .Cells(31).Value = Format(Val(dt1.Compute("Sum(Amount)", "")), "0.00")
                        .Cells(32).Value = Format(Val(dt1.Compute("Sum(TotalAmount)", "")), "0.00")
                        .Cells(37).Value = opbal
                        .Cells(38).Value = ClBal
                        .Cells(40).Value = crateopbal
                        .Cells(87).Value = lastbal
                        .Cells(81).Value = CrateName
                        .Cells(82).Value = CQty
                        .Cells(85).Value = SingleCrate
                        'If drcr = "Cr" Then
                        '    .Cells(38).Value = Val(Val(opbal)) - Val(dt1.Compute("Sum(TotalAmount)", "")) & " Cr"
                        'Else
                        '    .Cells(38).Value = Val(Val(dt1.Compute("Sum(TotalAmount)", "")) + Val(opbal)) & " Dr"
                        'End If
                        .Cells(39).Value = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'"))
                        dt2 = clsFun.ExecDataTable("Select  ('Last Receipt Rs. : '|| Vouchers.BasicAmount || ' On : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastReceipt,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC limit 3 ;")
                        ' dt2 = clsFun.ExecDataTable("Select Top 2 'Last Receipt Rs. : ' + cstr(Vouchers.TotalAmount) + ' On : '+Cstr(Vouchers.Entrydate) as lastpayment,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC;")
                        For j = 0 To dt2.Rows.Count - 1
                            .Cells(36).Value = .Cells(36).Value & dt2.Rows(j)("lastReceipt").ToString() & vbCrLf

                        Next
                        dt2.Dispose()
                        
                        '''''''''''''''''''''''''''''''''''''''''''''''''''

                        For j = 0 To dt1.Rows.Count - 1
                            Application.DoEvents()
                            .Cells(1).Value = Format(dt1.Rows(j)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = .Cells(2).Value & dt1.Rows(j)("Itemname").ToString() & vbCrLf
                            .Cells(3).Value = dt1.Rows(j)("AccountName").ToString()
                            .Cells(4).Value = .Cells(4).Value & Format(Val(dt1.Rows(j)("nug").ToString()), "0.00") & vbCrLf
                            .Cells(5).Value = .Cells(5).Value & Format(Val(dt1.Rows(j)("Weight").ToString()), "0.00") & vbCrLf
                            .Cells(6).Value = .Cells(6).Value & Format(Val(dt1.Rows(j)("Rate").ToString()), "0.00") & vbCrLf
                            .Cells(7).Value = .Cells(7).Value & dt1.Rows(j)("Per").ToString() & vbCrLf
                            .Cells(8).Value = .Cells(8).Value & Format(Val(dt1.Rows(j)("Amount").ToString()), "0.00") & vbCrLf
                            .Cells(9).Value = .Cells(9).Value & Format(Val(dt1.Rows(j)("Charges").ToString()), "0.00") & vbCrLf
                            .Cells(10).Value = .Cells(10).Value & Format(Val(dt1.Rows(j)("TotalAmount").ToString()), "0.00") & vbCrLf
                            .Cells(11).Value = .Cells(11).Value & Format(Val(dt1.Rows(j)("CommPer").ToString()), "0.00") & vbCrLf
                            .Cells(12).Value = .Cells(12).Value & Format(Val(dt1.Rows(j)("CommAmt").ToString()), "0.00") & vbCrLf
                            .Cells(13).Value = .Cells(13).Value & Format(Val(dt1.Rows(j)("MPer").ToString()), "0.00") & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & Format(Val(dt1.Rows(j)("MAmt").ToString()), "0.00") & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & Format(Val(dt1.Rows(j)("RdfPer").ToString()), "0.00") & vbCrLf
                            .Cells(16).Value = .Cells(16).Value & Format(Val(dt1.Rows(j)("RdfAmt").ToString()), "0.00") & vbCrLf
                            .Cells(17).Value = .Cells(17).Value & Format(Val(dt1.Rows(j)("Tare").ToString()), "0.00") & vbCrLf
                            .Cells(18).Value = .Cells(18).Value & Format(Val(dt1.Rows(j)("TareAmt").ToString()), "0.00") & vbCrLf
                            .Cells(19).Value = .Cells(19).Value & Format(Val(dt1.Rows(j)("Labour").ToString()), "0.00") & vbCrLf
                            .Cells(20).Value = .Cells(20).Value & Format(Val(dt1.Rows(j)("LabourAmt").ToString()), "0.00") & vbCrLf
                            If dt1.Rows(j)("mainTainCrate").ToString() = "Y" Then
                                .Cells(21).Value = .Cells(21).Value & dt1.Rows(j)("CrateMarka").ToString() & vbCrLf
                                .Cells(22).Value = .Cells(22).Value & dt1.Rows(j)("CrateQty").ToString() & vbCrLf
                            End If

                            .Cells(34).Value = .Cells(34).Value & dt1.Rows(j)("OtherName").ToString() & vbCrLf
                            .Cells(35).Value = .Cells(35).Value & dt1.Rows(j)("AccountNameOther").ToString() & vbCrLf
                        Next
                    End With
                End If
            End If
        Next i
        pnlWait.Visible = False
        dt.Clear()
        dt1.Clear()
        dt2.Clear()
    End Sub

    Private Sub rowColums1()
        With tmpgrid
            .ColumnCount = 105
            .Columns(0).Name = "ID" : .Columns(0).Visible = False
            .Columns(1).Name = "Date" : .Columns(1).Width = 95
            .Columns(2).Name = "ItemName1" : .Columns(2).Width = 159
            .Columns(3).Name = "Customer1" : .Columns(3).Width = 159
            .Columns(4).Name = "Nug1" : .Columns(4).Width = 59
            .Columns(5).Name = "Kg1" : .Columns(5).Width = 59
            .Columns(6).Name = "Rate1" : .Columns(6).Width = 69
            .Columns(7).Name = "Per1" : .Columns(7).Width = 76
            .Columns(8).Name = "Net1" : .Columns(8).Width = 90
            .Columns(9).Name = "Charges1" : .Columns(9).Width = 86
            .Columns(10).Name = "Total1" : .Columns(10).Width = 90
            .Columns(11).Name = "ComPer1" : .Columns(11).Width = 50
            .Columns(12).Name = "CommissionAmount1" : .Columns(12).Width = 95
            .Columns(13).Name = "Mper1" : .Columns(13).Width = 159
            .Columns(14).Name = "MAmount1" : .Columns(14).Width = 159
            .Columns(15).Name = "rdfper1" : .Columns(15).Width = 59
            .Columns(16).Name = "rdfAmount1" : .Columns(16).Width = 59
            .Columns(17).Name = "tare1" : .Columns(17).Width = 69
            .Columns(18).Name = "tareAmount1" : .Columns(18).Width = 76
            .Columns(19).Name = "Labour1" : .Columns(19).Width = 90
            .Columns(20).Name = "LabourAmount1" : .Columns(20).Width = 86
            .Columns(21).Name = "CrateName1" : .Columns(21).Width = 90
            .Columns(22).Name = "CrateQty1" : .Columns(22).Width = 90
            .Columns(23).Width = 95 : .Columns(24).Name = "SumKg"
            .Columns(24).Width = 159 : .Columns(25).Name = "SumComAmot"
            .Columns(25).Width = 159 : .Columns(26).Name = "SumMAmount"
            .Columns(26).Width = 59
            .Columns(27).Name = "SumRdfAmount"
            .Columns(27).Width = 59
            .Columns(28).Name = "SumtareAmount"
            .Columns(28).Width = 69
            .Columns(29).Name = "SumLabourAmount"
            .Columns(29).Width = 76
            .Columns(30).Name = "SumChargesAmount"
            .Columns(30).Width = 90
            .Columns(31).Name = "SumBasicAmount"
            .Columns(31).Width = 86
            .Columns(32).Name = "SumTotalAmount"
            .Columns(32).Width = 86
            .Columns(33).Name = "SumCrateAmount"
            .Columns(33).Width = 90
            .Columns(34).Name = "otherItemName"
            .Columns(34).Width = 90
            .Columns(35).Name = "otherAccountName"
            .Columns(35).Width = 69
            .Columns(36).Name = "LastPayment"
            .Columns(36).Width = 76
            .Columns(37).Name = ""
            .Columns(37).Width = 90
            .Columns(38).Name = ""
            .Columns(38).Width = 86
            .Columns(39).Name = ""
            .Columns(39).Width = 90
            .Columns(40).Name = ""
            .Columns(40).Width = 90
            .Columns(41).Name = "Date"
            .Columns(41).Width = 95
            .Columns(42).Name = "ItemName1"
            .Columns(42).Width = 159
            .Columns(43).Name = "Customer1"
            .Columns(43).Width = 159
            .Columns(44).Name = "Nug1"
            .Columns(44).Width = 59
            .Columns(45).Name = "Kg1"
            .Columns(45).Width = 59
            .Columns(46).Name = "Rate1"
            .Columns(46).Width = 69
            .Columns(47).Name = "Per1"
            .Columns(47).Width = 76
            .Columns(48).Name = "Net1"
            .Columns(48).Width = 90
            .Columns(49).Name = "Charges1"
            .Columns(49).Width = 86
            .Columns(50).Name = "Total1"
            .Columns(50).Width = 90
            .Columns(51).Name = "ComPer1"
            .Columns(51).Width = 50
            .Columns(52).Name = "CommissionAmount1"
            .Columns(52).Width = 95
            .Columns(53).Name = "Mper1"
            .Columns(53).Width = 159
            .Columns(54).Name = "MAmount1"
            .Columns(54).Width = 159
            .Columns(55).Name = "rdfper1"
            .Columns(55).Width = 59
            .Columns(56).Name = "rdfAmount1"
            .Columns(56).Width = 59
            .Columns(57).Name = "tare1"
            .Columns(57).Width = 69
            .Columns(58).Name = "tareAmount1"
            .Columns(58).Width = 76
            .Columns(59).Name = "Labour1"
            .Columns(59).Width = 90
            .Columns(60).Name = "LabourAmount1"
            .Columns(60).Width = 86
            .Columns(61).Name = "CrateName1"
            .Columns(61).Width = 90
            .Columns(62).Name = "CrateQty1"
            .Columns(62).Width = 90
            .Columns(63).Name = "SumNug"
            .Columns(63).Width = 95
            .Columns(64).Name = "SumKg"
            .Columns(64).Width = 159
            .Columns(65).Name = "SumComAmot"
            .Columns(65).Width = 159
            .Columns(66).Name = "SumMAmount"
            .Columns(66).Width = 59
            .Columns(67).Name = "SumRdfAmount"
            .Columns(67).Width = 59
            .Columns(68).Name = "SumtareAmount"
            .Columns(68).Width = 69
            .Columns(69).Name = "SumLabourAmount"
            .Columns(69).Width = 76
            .Columns(70).Name = "SumChargesAmount"
            .Columns(70).Width = 90
            .Columns(71).Name = "SumBasicAmount"
            .Columns(71).Width = 86
            .Columns(72).Name = "SumTotalAmount"
            .Columns(72).Width = 86
            .Columns(73).Name = "SumCrateAmount"
            .Columns(73).Width = 90
            .Columns(74).Name = "OtherItemname"
            .Columns(74).Width = 90
            .Columns(75).Name = "OtherAccountName"
            .Columns(75).Width = 69
            .Columns(76).Name = "LastPayment"
            .Columns(76).Width = 76
            .Columns(77).Name = ""
            .Columns(77).Width = 90
            .Columns(78).Name = ""
            .Columns(78).Width = 86
            .Columns(79).Name = ""
            .Columns(79).Width = 90
            .Columns(80).Name = "Ceatebalance"
            .Columns(80).Width = 90
            .Columns(81).Name = "SingleCrate NameLeft"
            .Columns(81).Width = 90
            .Columns(82).Name = "SingleCrate BalLeft"
            .Columns(82).Width = 90
            .Columns(83).Name = "SingleCrate NameRight"
            .Columns(83).Width = 90
            .Columns(84).Name = "SingleCrate BalRight"
            .Columns(84).Width = 90
            .Columns(85).Name = "TotalCrate OutOnly left"
            .Columns(85).Width = 90
            .Columns(86).Name = "TotalCrate OutOnly"
            .Columns(86).Width = 90
            .Columns(87).Name = "Cl Bal left"
            .Columns(87).Width = 90
            .Columns(88).Name = "cl Bal RIght"
            .Columns(88).Width = 90
            .Columns(89).Name = "Tot Crate Marka"
            .Columns(89).Width = 90
            .Columns(90).Name = "Tot Crate marka"
            .Columns(90).Width = 90
            .Columns(91).Name = "BillLeft"
            .Columns(91).Width = 90
            .Columns(92).Name = "BillRight"
            .Columns(92).Width = 90
            .Columns(93).Name = "LotNo"
            .Columns(93).Width = 90
            .Columns(94).Name = "LotNoRight"
            .Columns(94).Width = 90
            .Columns(95).Name = "Mobile1"
            .Columns(95).Width = 90
            .Columns(96).Name = "Mobile2"
            .Columns(96).Width = 90
            .Columns(97).Name = "LFNo"
            .Columns(97).Width = 90
            .Columns(98).Name = "Area"
            .Columns(98).Width = 90
            .Columns(99).Name = "City"
            .Columns(99).Width = 90
            .Columns(100).Name = "RMobile1"
            .Columns(100).Width = 90
            .Columns(101).Name = "RMobile2"
            .Columns(101).Width = 90
            .Columns(102).Name = "RLFNo"
            .Columns(102).Width = 90
            .Columns(103).Name = "RArea"
            .Columns(103).Width = 90
            .Columns(104).Name = "RCity"
            .Columns(104).Width = 90
        End With
    End Sub

    Private Sub tmpgrid_KeyDown(sender As Object, e As KeyEventArgs) Handles tmpgrid.KeyDown
        If e.KeyCode = Keys.Enter Then
            If tmpgrid.RowCount = 0 Then
                Exit Sub
            End If
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If dg1.RowCount = 0 Then
            MsgBox("There is no record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            If CDate(mskFromDate.Text).ToString("dd-MM-yyyy") <> CDate(MsktoDate.Text).ToString("dd-MM-yyyy") Then
                MessageBox.Show("You can print by one date only.", "Date Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                mskFromDate.Focus()
                Exit Sub
            End If
            ButtonControl() : IDGentate4Slips() : Save() : ButtonControl()
            If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
            Report_Viewer.printReport("\Bill4Hindi.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub
    Sub SlipsRecord(id)
        tmpgrid.Rows.Clear()
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim cnt As Integer = -1
        Dim acID As Integer = 0
        dt.Clear() : dt1.Clear() : dt2.Clear()
        If id <> "" Then
            id = " and Accountid in (" & id & ")"
        End If
        If ckJoin.Checked = False Then
            If ckCashBankBills.Checked = True Then
                dt = clsFun.ExecDataTable("Select AccountName,Accountid FROM BillPrints WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & " group by AccountName,Accountid order by AccountName")
            Else
                dt = clsFun.ExecDataTable("Select AccountName,Accountid FROM BillPrints WHERE  Accountid not in(7)  and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & " group by AccountName,Accountid order by AccountName")
            End If
        Else
            If ckCashBankBills.Checked = True Then
                dt = clsFun.ExecDataTable("Select AccountName,Accountid FROM BillPrints2 WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & " group by AccountName,Accountid order by AccountName")
            Else
                dt = clsFun.ExecDataTable("Select AccountName,Accountid FROM BillPrints2 WHERE  Accountid not in(7)  and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & " group by AccountName,Accountid order by AccountName")
            End If
        End If
    
        pb1.Minimum = 0
        Application.DoEvents()
        ' If dt.Rows.Count = 0 Then Exit Sub
        For i = 0 To dt.Rows.Count - 1
            If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub

            pb1.Maximum = dt.Rows.Count - 1
            pb1.Value = i
            Dim opbal As String = ""
            Dim Dayopamount As Decimal = 0.0
            Dim ClBal As String = ""
            ''''''''''''''''''''' Opening Balance'''''''''''''''''''''''''''''''''''
            opbal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                     " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID")) & " Order by upper(AccountName) ;"))

            ''''''''''''''''''''closing balance'''''''''''''''''''''''''
            ClBal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                     " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID")) & " Order by upper(AccountName) ;"))

            TodaysCredit = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and EntryDate = '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"))
            todaysDebit = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and EntryDate = '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"))
            If ClBal < 0 Then
                lastbal = Format(Math.Abs(Val(Val(ClBal + todaysDebit) - TodaysCredit)), "0.00") & " Cr"
            Else
                lastbal = Format(Math.Abs(Val(Val(ClBal - todaysDebit))), "0.00") & " Dr"
            End If
            acID = Val(dt.Rows(i)("AccountID").ToString())
            ''''''Total Crates Show''''''
            Dim U As Integer = 0
            Dim cratebal As String = String.Empty
            Dim CrateQty As String = String.Empty
            Dim CrateName As String = String.Empty
            Dim CQty As String = String.Empty
            Dim SingleCrate As String = String.Empty
            Dim dtcrate As New DataTable
            dtcrate = clsFun.ExecDataTable("Select CrateName,CrateName ||':'||" &
            " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') -" &
            " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) as Reciveable," &
                        " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') -" &
            " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) as DueCrates " &
            " FROM CrateVoucher as CV INNER JOIN Account_AcGrp AS ACG ON CV.AccountID = ACG.ID Where EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' Group by AccountID,CrateID Having DueCrates<>0 order by upper(ACG.AccountName);")
            Try
                If dtcrate.Rows.Count > 0 Then
                    For U = 0 To dtcrate.Rows.Count - 1
                        If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
                        cratebal = dtcrate.Rows(U)("Reciveable").ToString()
                        CrateQty = CrateQty & ", " & cratebal
                        CrateName = CrateName & dtcrate.Rows(U)("CrateName").ToString() & vbCrLf
                        CQty = CQty & dtcrate.Rows(U)("DueCrates").ToString() & vbCrLf
                        SingleCrate = Val(SingleCrate) + Val(dtcrate.Rows(U)("DueCrates").ToString())
                    Next
                    CrateQty = CrateQty.Trim().TrimStart(",")

                End If
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''
            crateopbal = Val(0)
            ssql = "Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,Qty as QtyIn,'0' as QtyOut from CrateVoucher where CrateType ='Crate In' " & IIf(Val(dt.Rows(i)("AccountID").ToString()) > 0, "and AccountID=" & Val(dt.Rows(i)("AccountID").ToString()) & "", "") & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'   union all" &
                " Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,'0' as QtyOut,Qty as QtyIn from CrateVoucher where CrateType ='Crate Out' " & IIf(Val(dt.Rows(i)("AccountID").ToString()) > 0, "and AccountID=" & Val(dt.Rows(i)("AccountID").ToString()) & "", "") & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'     "
            Dim cratetmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim cratetmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim cratetmpamt As String = Val(Val(crateopbal) + Val(cratetmpamtdr)) - Val(cratetmpamtcr)
            If cratetmpamt > 0 Then
                crateopbal = Val(cratetmpamt)
            Else
                crateopbal = -Val(cratetmpamt)
            End If
            If ckJoin.Checked = False Then
                dt1 = clsFun.ExecDataTable("Select * FROM BillPrints WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' order by AccountName")
            Else
                dt1 = clsFun.ExecDataTable("Select * FROM BillPrints2 WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' order by AccountName")

            End If
            If dt1.Rows.Count > 0 Then
                If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    Application.DoEvents()
                    .Cells(23).Value = Format(dt1.Compute("Sum(Nug)", ""), "0.")
                    .Cells(24).Value = Format(dt1.Compute("Sum(Weight)", ""), "0.00")
                    .Cells(25).Value = Format(dt1.Compute("Sum(CommAmt)", ""), "0.00")
                    .Cells(26).Value = Format(dt1.Compute("Sum(MAmt)", ""), "0.00")
                    .Cells(27).Value = Format(dt1.Compute("Sum(RdfAmt)", ""), "0.00")
                    .Cells(28).Value = Format(dt1.Compute("Sum(TareAmt)", ""), "0.00")
                    .Cells(29).Value = Format(dt1.Compute("Sum(LabourAmt)", ""), "0.00")
                    .Cells(30).Value = Format(dt1.Compute("Sum(Charges)", ""), "0.00")
                    .Cells(31).Value = Format(dt1.Compute("Sum(Amount)", ""), "0.00")
                    .Cells(32).Value = Format(dt1.Compute("Sum(TotalAmount)", ""), "0.00")
                    .Cells(40).Value = Val(cratetmpamt)
                    .Cells(37).Value = If(Val(opbal) >= 0, Format(Math.Abs(Val(opbal)), "0.00") & " Dr", Format(Math.Abs(Val(opbal)), "0.00") & " Cr")
                    .Cells(38).Value = If(Val(ClBal) >= 0, Format(Math.Abs(Val(ClBal)), "0.00") & " Dr", Format(Math.Abs(Val(ClBal)), "0.00") & " Cr")
                    .Cells(39).Value = Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(40).Value = crateopbal
                    .Cells(47).Value = CrateQty
                    .Cells(87).Value = lastbal
                    .Cells(43).Value = CrateName
                    .Cells(44).Value = CQty
                    .Cells(45).Value = SingleCrate
                    .Cells(48).Value = clsFun.ExecScalarInt("Select RowID From Transaction2 Where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' Group by AccountID,EntryDate Order by RowID")
                    dt2 = clsFun.ExecDataTable("Select  ('Last Receipt Rs. : '|| Vouchers.BasicAmount || ' On : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastReceipt,AccountID,TransType FROM Vouchers where TransType='Receipt' and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'  and Accountid=" & Val(dt.Rows(i)("AccountID").ToString()) & " ORDER BY Vouchers.Entrydate DESC limit 1 ;")

                    '  dt2 = clsFun.ExecDataTable("Select Top 2 'Last Receipt Rs. : ' + cstr(Vouchers.TotalAmount) + ' On : '+Cstr(Vouchers.Entrydate) as lastpayment,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC;")
                    For j = 0 To dt2.Rows.Count - 1
                        .Cells(36).Value = .Cells(36).Value & dt2.Rows(j)("lastReceipt").ToString() & vbCrLf
                    Next
                    dt3 = clsFun.ExecDataTable("Select  ('Last Amount Rs. : '|| Vouchers.BasicAmount || ' On : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastPayment,AccountID,TransType FROM Vouchers where TransType='Payment' and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and Accountid=" & Val(dt.Rows(i)("AccountID").ToString()) & " ORDER BY Vouchers.Entrydate DESC limit 5 ;")
                    For k = 0 To dt3.Rows.Count - 1
                        .Cells(46).Value = .Cells(46).Value & dt3.Rows(k)("lastPayment").ToString() & vbCrLf
                    Next
                    dt2.Dispose()
                    For j = 0 To dt1.Rows.Count - 1
                        Application.DoEvents()
                        .Cells(1).Value = Format(dt1.Rows(j)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = .Cells(2).Value & dt1.Rows(j)("Itemname").ToString() & vbCrLf
                        .Cells(3).Value = dt1.Rows(j)("AccountName").ToString()
                        .Cells(4).Value = .Cells(4).Value & Format(Val(dt1.Rows(j)("nug").ToString()), "0.00") & vbCrLf
                        .Cells(5).Value = .Cells(5).Value & Format(Val(dt1.Rows(j)("Weight").ToString()), "0.00") & vbCrLf
                        .Cells(6).Value = .Cells(6).Value & Format(Val(dt1.Rows(j)("Rate").ToString()), "0.00") & vbCrLf
                        .Cells(7).Value = .Cells(7).Value & dt1.Rows(j)("Per").ToString() & vbCrLf
                        .Cells(8).Value = .Cells(8).Value & Format(Val(dt1.Rows(j)("Amount").ToString()), "0.00") & vbCrLf
                        .Cells(9).Value = .Cells(9).Value & Format(Val(dt1.Rows(j)("Charges").ToString()), "0.00") & vbCrLf
                        .Cells(10).Value = .Cells(10).Value & Format(Val(dt1.Rows(j)("TotalAmount").ToString()), "0.00") & vbCrLf
                        .Cells(11).Value = .Cells(11).Value & dt1.Rows(j)("CommPer").ToString() & vbCrLf
                        .Cells(12).Value = .Cells(12).Value & dt1.Rows(j)("CommAmt").ToString() & vbCrLf
                        .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("MPer").ToString() & vbCrLf
                        .Cells(14).Value = .Cells(14).Value & dt1.Rows(j)("MAmt").ToString() & vbCrLf
                        .Cells(15).Value = .Cells(15).Value & dt1.Rows(j)("RdfPer").ToString() & vbCrLf
                        .Cells(16).Value = .Cells(16).Value & dt1.Rows(j)("RdfAmt").ToString() & vbCrLf
                        .Cells(17).Value = .Cells(17).Value & dt1.Rows(j)("Tare").ToString() & vbCrLf
                        .Cells(18).Value = .Cells(18).Value & dt1.Rows(j)("TareAmt").ToString() & vbCrLf
                        .Cells(19).Value = .Cells(19).Value & dt1.Rows(j)("Labour").ToString() & vbCrLf
                        .Cells(20).Value = .Cells(20).Value & dt1.Rows(j)("LabourAmt").ToString() & vbCrLf
                        .Cells(79).Value = .Cells(79).Value & Format(Val(dt1.Compute("Sum(Amount)", "")) + Val(dt1.Compute("Sum(LabourAmt)", "")), "0.00") & vbCrLf
                        .Cells(80).Value = .Cells(80).Value & Format(Val(dt1.Rows(j)("Amount").ToString()) + Val(dt1.Rows(j)("LabourAmt").ToString()), "0.00") & vbCrLf
                        .Cells(81).Value = .Cells(81).Value & Format(Val(dt1.Compute("Sum(Charges)", "")) - Val(dt1.Compute("Sum(LabourAmt)", "")), "0.00") & vbCrLf
                        .Cells(88).Value = .Cells(88).Value & dt1.Rows(j)("OnWeight").ToString() & vbCrLf
                        .Cells(93).Value = .Cells(93).Value & Val(dt1.Rows(j)("Lot").ToString()) & vbCrLf
                        .Cells(76).Value = .Cells(76).Value & Val(dt1.Rows(j)("GrossWeight").ToString()) & vbCrLf
                        .Cells(77).Value = .Cells(77).Value & Val(dt1.Rows(j)("Cut").ToString()) & vbCrLf
                        .Cells(95).Value = dt1.Rows(j)("MobileNo1").ToString()
                        .Cells(96).Value = dt1.Rows(j)("MobileNo2").ToString()
                        .Cells(97).Value = dt1.Rows(j)("Area").ToString()
                        .Cells(98).Value = dt1.Rows(j)("City").ToString()
                        .Cells(99).Value = dt1.Rows(j)("LFNo").ToString()
                        If dt1.Rows(j)("mainTaincrate").ToString() = "Y" Then
                            .Cells(21).Value = .Cells(21).Value & dt1.Rows(j)("CrateMarka").ToString() & vbCrLf
                            .Cells(22).Value = .Cells(22).Value & dt1.Rows(j)("CrateQty").ToString() & vbCrLf
                        Else
                            .Cells(21).Value = "" & vbCrLf
                            .Cells(22).Value = "" & vbCrLf
                        End If
                        .Cells(34).Value = .Cells(34).Value & dt1.Rows(j)("OtherName").ToString() & vbCrLf
                        .Cells(35).Value = dt1.Rows(j)("AccountNameOther").ToString()
                    Next

                End With
            End If
        Next i
        dt.Clear() : dt1.Clear() : dt2.Clear()
    End Sub
    
    Sub Day2Day(id)
        If tmpgrid.Rows.Count = 0 Then rowColums1()
        tmpgrid.Rows.Clear()
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim cnt As Integer = -1
        Dim acID As Integer = 0
        dt.Clear() : dt1.Clear() : dt2.Clear()
        If id <> "" Then
            id = " and Accountid in (" & id & ")"
        End If
        If ckCashBankBills.Checked = True Then
            dt = clsFun.ExecDataTable("Select AccountName,Accountid FROM BillPrints WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & " group by Accountid order by AccountName,EntryDate")
        Else
            dt = clsFun.ExecDataTable("Select AccountName,Accountid FROM BillPrints WHERE  Accountid not in(7)  and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & " group by Accountid order by AccountName,EntryDate")
        End If
        pb1.Minimum = 0
        ' If dt.Rows.Count = 0 Then Exit Sub
        For i = 0 To dt.Rows.Count - 1
            Application.DoEvents()
            If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
            pb1.Maximum = dt.Rows.Count - 1
            pb1.Value = i
            Dim opbal As String = ""
            Dim Dayopamount As Decimal = 0.0
            Dim ClBal As String = ""
        
            ''''''''''''''''''''' Opening Balance'''''''''''''''''''''''''''''''''''
            opbal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                     " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID")) & " Order by upper(AccountName) ;"))

            ''''''''''''''''''''closing balance'''''''''''''''''''''''''

            ClBal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                     " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" &
                                     " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID")) & " Order by upper(AccountName) ;"))

            TodaysCredit = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and EntryDate = '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"))
            todaysDebit = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and EntryDate = '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"))
            If ClBal < 0 Then
                lastbal = Format(Math.Abs(Val(Val(ClBal + todaysDebit) - TodaysCredit)), "0.00") & " Cr"
            Else
                lastbal = Format(Math.Abs(Val(Val(ClBal - todaysDebit))), "0.00") & " Dr"
            End If


            acID = Val(dt.Rows(i)("AccountID").ToString())
            ''''''Total Crates Show''''''
            Dim U As Integer = 0
            Dim cratebal As String = String.Empty
            Dim CrateQty As String = String.Empty
            Dim CrateName As String = String.Empty
            Dim CQty As String = String.Empty
            Dim SingleCrate As String = String.Empty
            Dim dtcrate As New DataTable
            dtcrate = clsFun.ExecDataTable("Select CrateName,CrateName ||':'||" &
            " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') -" &
            " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) as Reciveable," &
                        " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') -" &
            " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) as DueCrates " &
            " FROM CrateVoucher as CV INNER JOIN Account_AcGrp AS ACG ON CV.AccountID = ACG.ID Where EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' Group by AccountID,CrateID Having DueCrates>0 order by upper(ACG.AccountName);")
            Try
                If dtcrate.Rows.Count > 0 Then
                    For U = 0 To dtcrate.Rows.Count - 1
                        If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
                        cratebal = dtcrate.Rows(U)("Reciveable").ToString()
                        CrateQty = CrateQty & ", " & cratebal
                        CrateName = CrateName & dtcrate.Rows(U)("CrateName").ToString() & vbCrLf
                        CQty = CQty & dtcrate.Rows(U)("DueCrates").ToString() & vbCrLf
                        SingleCrate = Val(SingleCrate) + Val(dtcrate.Rows(U)("DueCrates").ToString())
                    Next
                    CrateQty = CrateQty.Trim().TrimStart(",")

                End If
            Catch ex As Exception

            End Try
            '''''''''''''''''''''''''''''''
            crateopbal = Val(0)
            ssql = "Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,Qty as QtyIn,'0' as QtyOut from CrateVoucher where CrateType ='Crate In' " & IIf(Val(acID) > 0, "and AccountID=" & Val(acID) & "", "") & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'   union all" &
                " Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,'0' as QtyOut,Qty as QtyIn from CrateVoucher where CrateType ='Crate Out' " & IIf(Val(acID) > 0, "and AccountID=" & Val(acID) & "", "") & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'     "
            Dim cratetmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(acID) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim cratetmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(acID) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim cratetmpamt As String = Val(Val(crateopbal) + Val(cratetmpamtdr)) - Val(cratetmpamtcr)
            If cratetmpamt > 0 Then
                crateopbal = Val(cratetmpamt)
            Else
                crateopbal = -Val(cratetmpamt)
            End If
            dt1 = clsFun.ExecDataTable("Select * FROM BillPrints WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & dt.Rows(i)(1) & "'order by AccountName,EntryDate")
            If dt1.Rows.Count > 0 Then
                For j = 0 To dt1.Rows.Count - 1
                    If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
                    tmpgrid.Rows.Add()
                    cnt = cnt + 1
                    With tmpgrid.Rows(cnt)
                        Application.DoEvents()
                        .Cells(23).Value = Format(dt1.Compute("Sum(Nug)", ""), "0.00")
                        .Cells(24).Value = Format(dt1.Compute("Sum(Weight)", ""), "0.00")
                        .Cells(25).Value = Format(dt1.Compute("Sum(CommAmt)", ""), "0.00")
                        .Cells(26).Value = Format(dt1.Compute("Sum(MAmt)", ""), "0.00")
                        .Cells(27).Value = Format(dt1.Compute("Sum(RdfAmt)", ""), "0.00")
                        .Cells(28).Value = Format(dt1.Compute("Sum(TareAmt)", ""), "0.00")
                        .Cells(29).Value = Format(dt1.Compute("Sum(LabourAmt)", ""), "0.00")
                        .Cells(30).Value = Format(dt1.Compute("Sum(Charges)", ""), "0.00")
                        .Cells(31).Value = Format(dt1.Compute("Sum(Amount)", ""), "0.00")
                        .Cells(32).Value = Format(dt1.Compute("Sum(TotalAmount)", ""), "0.00")
                        .Cells(40).Value = Val(cratetmpamt)
                        .Cells(37).Value = If(Val(opbal) >= 0, Format(Math.Abs(Val(opbal)), "0.00") & " Dr", Format(Math.Abs(Val(opbal)), "0.00") & " Cr")
                        .Cells(38).Value = If(Val(ClBal) >= 0, Format(Math.Abs(Val(ClBal)), "0.00") & " Dr", Format(Math.Abs(Val(ClBal)), "0.00") & " Cr")
                        .Cells(39).Value = Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID")).ToString() & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(40).Value = crateopbal
                        .Cells(47).Value = CrateQty
                        .Cells(48).Value = Format(Val(clsFun.ExecScalarStr("Select sum(Amount)+ sum(Commamt) + sum(MAmt)+ sum(RdfAmt)+ sum(LabourAmt)+sum(RoundOff) as tot from Transaction2 where CommAmt = 0 and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and Accountid=" & dt.Rows(i)(1) & "")), "0.00")
                        .Cells(49).Value = Format(Val(clsFun.ExecScalarStr("Select sum(Amount)+ sum(Commamt) + sum(MAmt)+ sum(RdfAmt)+ sum(LabourAmt)+sum(RoundOff) as tot from Transaction2 where CommAmt <> 0 and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and Accountid=" & dt.Rows(i)(1) & "")), "0.00")
                        .Cells(43).Value = CrateName
                        .Cells(44).Value = CQty
                        .Cells(45).Value = SingleCrate
                        '.Cells(50).Value = Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt1.Rows(i)("AccountID")).ToString() & " and EntryDate = '" & CDate(dt1.Rows(i)("EntryDate").ToString()).ToString("yyyy-MM-dd") & "'")), "0.00")
                        dt2 = clsFun.ExecDataTable("Select  ('Last Receipt Rs. : '|| Vouchers.BasicAmount || ' On : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastReceipt,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC limit 1 ;")
                        '  dt2 = clsFun.ExecDataTable("Select Top 2 'Last Receipt Rs. : ' + cstr(Vouchers.TotalAmount) + ' On : '+Cstr(Vouchers.Entrydate) as lastpayment,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC;")
                        For l = 0 To dt2.Rows.Count - 1
                            If dt2.Rows.Count > 0 Then
                                .Cells(36).Value = .Cells(36).Value & dt2.Rows(l)("lastReceipt").ToString() & vbCrLf
                            End If
                        Next
                        dt3 = clsFun.ExecDataTable("Select  ('Last Amount Rs. : '|| Vouchers.BasicAmount || ' On : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastPayment,AccountID,TransType FROM Vouchers where TransType='Payment' and Accountid=" & dt.Rows(i)(1) & " ORDER BY Vouchers.Entrydate DESC limit 1 ;")
                        For k = 0 To dt3.Rows.Count - 1
                            If dt3.Rows.Count > 0 Then
                                .Cells(46).Value = .Cells(46).Value & dt3.Rows(k)("lastPayment").ToString() & vbCrLf
                            End If
                        Next
                        dt2.Dispose()
                        '   For j = 0 To dt1.Rows.Count - 1
                        Application.DoEvents()
                        .Cells(1).Value = Format(dt1.Rows(j)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt1.Rows(j)("Itemname").ToString()
                        .Cells(3).Value = dt1.Rows(j)("AccountName").ToString()
                        .Cells(4).Value = Format(Val(dt1.Rows(j)("nug").ToString()), "0.00")
                        .Cells(5).Value = Format(Val(dt1.Rows(j)("Weight").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt1.Rows(j)("Rate").ToString()), "0.00")
                        .Cells(7).Value = dt1.Rows(j)("Per").ToString() & vbCrLf
                        .Cells(8).Value = Format(Val(dt1.Rows(j)("Amount").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt1.Rows(j)("Charges").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt1.Rows(j)("TotalAmount").ToString()), "0.00")
                        .Cells(11).Value = dt1.Rows(j)("CommPer").ToString()
                        .Cells(12).Value = dt1.Rows(j)("CommAmt").ToString()
                        .Cells(13).Value = dt1.Rows(j)("MPer").ToString()
                        .Cells(14).Value = dt1.Rows(j)("MAmt").ToString()
                        .Cells(15).Value = dt1.Rows(j)("RdfPer").ToString()
                        .Cells(16).Value = dt1.Rows(j)("RdfAmt").ToString()
                        .Cells(17).Value = dt1.Rows(j)("Tare").ToString()
                        .Cells(18).Value = dt1.Rows(j)("TareAmt").ToString()
                        .Cells(19).Value = dt1.Rows(j)("Labour").ToString()
                        .Cells(20).Value = dt1.Rows(j)("LabourAmt").ToString()
                        .Cells(93).Value = Val(dt1.Rows(j)("Lot").ToString())
                        If dt1.Rows(j)("mainTainCrate").ToString() = "Y" Then
                            .Cells(21).Value = .Cells(21).Value & dt1.Rows(j)("CrateMarka").ToString() & vbCrLf
                            .Cells(22).Value = .Cells(22).Value & dt1.Rows(j)("CrateQty").ToString() & vbCrLf
                        End If
                        .Cells(34).Value = dt1.Rows(j)("OtherName").ToString()
                        .Cells(35).Value = dt1.Rows(j)("AccountNameOther").ToString()
                    End With
                Next
            End If
        Next i
        dt.Clear() : dt1.Clear() : dt2.Clear()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnSlips.Click
        If dg1.RowCount = 0 Then
            MsgBox("There is No record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            'If CDate(mskFromDate.Text).ToString("dd-MM-yyyy") <> CDate(MsktoDate.Text).ToString("dd-MM-yyyy") Then
            '    MessageBox.Show("You can print by one date only.", "Date Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    mskFromDate.Focus()
            '    Exit Sub
            'End If
            ButtonControl() : pnlWait.Visible = True
            IDGentateSlips() : PrintSlips()
            pnlWait.Visible = False : ButtonControl()
            If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
            Report_Viewer.printReport("\Slips.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub
    Private Sub btnSlipsHindi_Click(sender As Object, e As EventArgs) Handles btnSlipsHindi.Click
        If dg1.RowCount = 0 Then
            MsgBox("There is No record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            If CDate(mskFromDate.Text).ToString("dd-MM-yyyy") <> CDate(MsktoDate.Text).ToString("dd-MM-yyyy") Then
                MessageBox.Show("You can print by one date only.", "Date Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                mskFromDate.Focus()
                Exit Sub
            End If
            pnlWait.Visible = True
            ButtonControl()
            IDGentateSlips() : PrintSlips()
            ButtonControl()
            pnlWait.Visible = False
            If Application.OpenForms().OfType(Of Print_Bills)().Any(Function(f) f.Visible = False) Then Exit Sub
            Report_Viewer.printReport("\SlipsHindi.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        DGSearchAccounts() : ShowAccounts()
        pnlSearch.Visible = True
        txtSearchAccount.Focus()
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        ' Progress()
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub btnSms_Click(sender As Object, e As EventArgs) Handles btnSms.Click
        Dim Acid As String = String.Empty
        Dim MobieNo As String = String.Empty
        For i As Integer = 0 To dg1.Rows.Count - 1
            If Val(dg1.Rows(i).Cells(0).Value.ToString()) > 0 Then
                Acid = Acid & Val(dg1.Rows(i).Cells(23).Value.ToString()) & ","
            End If
        Next
        Acid = Acid.Remove(Acid.LastIndexOf(","))
        Dim tempdt As DataTable = clsFun.ExecDataTable("Select Mobile1 from Accounts where id in(" & Acid & ")")
        For i As Integer = 0 To tempdt.Rows.Count - 1
            If Len(tempdt.Rows(i)("Mobile1").ToString()) = 10 Then
                MobieNo = MobieNo & tempdt.Rows(i)("Mobile1").ToString() & ","
            End If
        Next

        MobieNo = MobieNo.Remove(MobieNo.LastIndexOf(","))
        Dim resp As String = Sms.SendSms(MobieNo)
        If resp = "Message Submitted" Then
            MsgBox("Message send")
        End If
    End Sub
    Private Sub DgWhatsapp_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgWhatsapp.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 1 Then
            'Loop to verify whether all row CheckBoxes are checked or not.
            Dim isChecked As Boolean = True
            For Each row As DataGridViewRow In DgWhatsapp.Rows
                If Convert.ToBoolean(row.Cells("chk").EditedFormattedValue) = False Then
                    isChecked = True
                    Exit For
                End If
            Next
            WhatsappCheckBox.Checked = isChecked
        End If
    End Sub
    Private Sub dgAccounts_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgAccounts.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 1 Then
            'Loop to verify whether all row CheckBoxes are checked or not.
            Dim isChecked As Boolean = True
            For Each row As DataGridViewRow In dgAccounts.Rows
                If Convert.ToBoolean(row.Cells("chk").EditedFormattedValue) = False Then
                    isChecked = True
                    Exit For
                End If
            Next
            headerCheckBox.Checked = isChecked
        End If
    End Sub

    Private Sub btnok_Click(sender As Object, e As EventArgs) Handles btnok.Click
        tmpgrid.Rows.Clear()
        Dim id As String = String.Empty
        Dim checkBox As DataGridViewCheckBoxCell
        For Each row As DataGridViewRow In dgAccounts.Rows
            checkBox = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            If checkBox.Value = True Then
                id = id & row.Cells(1).Value & ","
            End If
        Next
        If id = "" Then
            retrive(id)
            Exit Sub
        Else
            id = id.Remove(id.LastIndexOf(","))
            retrive(id)
        End If
        pnlSearch.Visible = False
    End Sub

    Private Sub txtSearchAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearchAccount.KeyDown
        If e.KeyCode = Keys.Down Then dgAccounts.Focus()
        If e.KeyData = Keys.Enter Then btnok.PerformClick()
    End Sub

    Private Sub txtSearchAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearchAccount.KeyUp
        Dim searchValue As String = txtSearchAccount.Text.Trim().ToLower()
        For Each row As DataGridViewRow In dgAccounts.Rows
            If Not row.IsNewRow Then
                Dim cellValue As String = row.Cells(2).Value.ToString().ToLower()
                If cellValue.StartsWith(searchValue) Then
                    row.Visible = True
                Else
                    row.Visible = False
                End If
            End If
        Next
    End Sub

    Private Sub dgAccounts_KeyDown(sender As Object, e As KeyEventArgs) Handles dgAccounts.KeyDown
        If e.KeyCode = Keys.Back Then txtSearchAccount.Focus()
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Exit Sub
        End If
    End Sub

    Private Sub ckJoin_CheckedChanged(sender As Object, e As EventArgs) Handles ckJoin.CheckedChanged

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If CDate(mskFromDate.Text).ToString("dd-MM-yyyy") <> CDate(MsktoDate.Text).ToString("dd-MM-yyyy") Then
            MessageBox.Show("You can print by one date only.", "Date Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error)
            mskFromDate.Focus()
            Exit Sub
        End If
        If tmpgrid.Rows.Count = 0 Then rowColums1()
        If MessageBox.Show("Are you Sure to Want to Print Without Cash...??", "Print Cash/ No Cash...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            ButtonControl() : printDetailedWithOutCash() : ButtonControl()
        Else
            ButtonControl() : printDetailed() : ButtonControl()
        End If


        'clsFun.changeCompany()
        Save()
        Report_Viewer.printReport("\Nakal.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
    Private Sub printDetailed()
        pnlWait.Visible = True
        Application.DoEvents()
        tmpgrid.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim opbal As String = ""
        Dim ClBal As String = ""
        Dim count As Integer = 0
        Dim ssql As String = String.Empty
        Dim lastval As Integer = 0
        'ssql = "Select t2.EntryDate as entrydate, Ac.AccountName as AccountName,Ac.otherName as AccountHindiName,ac.id as id, i.othername as hname ,t2.Charges as Charges,t2.ItemName as ItemName,t2.Nug as nug,t2.Weight as Weight,t2.Rate as rate,t2.Per as per, t2.Amount as amount,t2.CommAmt as commamt,t2.MAmt as mamt,t2.RdfAmt as RdfAmt,t2.TareAmt as TareAmt,t2.labouramt as labour,  t2.TotalAmount as TotalAmount,t2.Cratemarka as Cratemarka,t2.CrateQty as CrateQty ,v.TransType as TransType    from Account_acgrp ac " & _
        '    "inner join Transaction2 t2 on ac.id=t2.accountid Inner join items i on i.id=t2.itemid left join vouchers v on v.id =t2.voucherid where (ac.groupid =16 or ac.undergroupid=16)  " & _
        '    "and  t2.EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'  order by ac.accountname "
        Dim CashTotalAmount As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  AccountID in (Select ID From Accounts Where GroupID=11) and TransType Not In('On Sale','Store Transfer') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditTotalAmount As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  AccountID in (Select ID From Accounts Where GroupID<>11) and TransType Not In('On Sale','Store Transfer')  and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalAmount As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where TransType Not In('On Sale','Store Transfer') and  EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalNug As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(nug) as tot from Transaction2 Where TransType Not In('On Sale','Store Transfer') and  EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalWeight As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Weight) as tot from Transaction2 Where TransType Not In('On Sale','Store Transfer') and  EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalComm As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(CommAmt) as tot from Transaction2 where  TransType Not In('On Sale','Store Transfer') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalMandiTax As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(MAmt) as tot from Transaction2 where TransType Not In('On Sale','Store Transfer') and  EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalRDF As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(RdfAmt) as tot from Transaction2 where  TransType Not In('On Sale','Store Transfer') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalTare As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(TareAmt) as tot from Transaction2 where TransType Not In('On Sale','Store Transfer') and  EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalLabour As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Labour) as tot from Transaction2 where TransType Not In('On Sale','Store Transfer') and  EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalAmount2 As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where TransType Not In('On Sale','Store Transfer') and  EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalRoundOff As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Roundoff) as tot from Transaction2 where TransType Not In('On Sale','Store Transfer') and  EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CashAmount As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Transaction2 where  AccountID in (Select ID From Accounts Where GroupID=11) and TransType Not In('On Sale','Store Transfer') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditAMount As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Transaction2 where  accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditNug As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(nug) as tot from Transaction2 where AccountID in (Select ID From Accounts Where GroupID<>11) and TransType Not In('On Sale','Store Transfer') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditWeight As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Weight) as tot from Transaction2 where AccountID in (Select ID From Accounts Where GroupID<>11) and TransType Not In('On Sale','Store Transfer')  and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditComm As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(CommAmt) as tot from Transaction2 where AccountID in (Select ID From Accounts Where GroupID<>11) and TransType Not In('On Sale','Store Transfer')  and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditMamt As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(MAmt) as tot from Transaction2 where AccountID in (Select ID From Accounts Where GroupID<>11) and TransType Not In('On Sale','Store Transfer') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditRDF As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(RdfAmt) as tot from Transaction2 where AccountID in (Select ID From Accounts Where GroupID<>11) and TransType Not In('On Sale','Store Transfer') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditTare As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(TareAmt) as tot from Transaction2 where AccountID in (Select ID From Accounts Where GroupID<>11) and TransType Not In('On Sale','Store Transfer') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreDitLabour = Format(Val(clsFun.ExecScalarStr("Select sum(Labour) as tot from Transaction2 where AccountID in (Select ID From Accounts Where GroupID<>11) and TransType Not In('On Sale','Store Transfer') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalChargesWithoutComm As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Amount)+ sum(Commamt) + sum(MAmt)+ sum(RdfAmt)+ sum(LabourAmt)+sum(RoundOff) as tot from Transaction2 where CommAmt = 0 and TransType Not In('On Sale','Store Transfer') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalChargesWithComm As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Amount)+ sum(Commamt) + sum(MAmt)+ sum(RdfAmt)+ sum(LabourAmt)+sum(RoundOff) as tot from Transaction2 where CommAmt <> 0 and TransType Not In('On Sale','Store Transfer') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalCrates As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(CrateQty) as CrateQty  from Transaction2 where  TransType Not In('On Sale','Store Transfer') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")))

        ssql = "Select *  from Transaction2 Where  AccountID in (Select ID From Accounts Where GroupID<>11) and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and TransType Not In('On Sale','Store Transfer')  order by AccountName "
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            ' pnlWait.Visible = True
            '  Application.DoEvents()
            pb1.Minimum = 0
            For i = 0 To dt.Rows.Count - 1
                'pnlWait.Visible = True
                pb1.Maximum = dt.Rows.Count - 1
                pb1.Value = i
                ''''''''''''''''''''' Opening Balance'''''''''''''''''''''''''''''''''''
                opbal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                         "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                         " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                         " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID")) & " Order by upper(AccountName) ;"))

                ''''''''''''''''''''closing balance'''''''''''''''''''''''''

                ClBal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" &
                                         "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                         " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" &
                                         " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID")) & " Order by upper(AccountName) ;"))

                Dim AccountCreditAmt As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                Dim HindiAccount As String = clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & Val(dt.Rows(i)("AccountID").ToString()) & "")
                Dim HindiItem As String = clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & Val(dt.Rows(i)("ItemID").ToString()) & "") & IIf(Val(dt.Rows(i)("CrateQty").ToString()) = 0, "", " (" & dt.Rows(i)("CrateMarka").ToString() & " : " & dt.Rows(i)("CrateQty").ToString() & ")")
                Dim STDTotalCharges As Decimal = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalCharges) From Vouchers Where TransType='Standard Sale' and AccountID='" & Val(dt.Rows(i)("AccountID")) & "'")), "0.00")
                Dim AccountCrates As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(CrateQty) as CrateQty  from Transaction2 where AccountID='" & Val(dt.Rows(i)("AccountID")) & "' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")))

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                tmpgrid.Rows.Add()
                With tmpgrid.Rows(i)
                    'If dt.Rows(i)("Id").ToString() = 878 Then MsgBox("A")
                    .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
                    .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(2).Value = If(Val(opbal) >= 0, Format(Math.Abs(Val(opbal)), "0.00") & " Dr", Format(Math.Abs(Val(opbal)), "0.00") & " Cr")
                    .Cells(3).Value = dt.Rows(i)("ItemName").ToString() & IIf(Val(dt.Rows(i)("CrateQty").ToString()) = 0, "", " (" & dt.Rows(i)("CrateMarka").ToString() & " : " & dt.Rows(i)("CrateQty").ToString() & ")")
                    .Cells(4).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                    .Cells(5).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                    .Cells(6).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                    .Cells(7).Value = dt.Rows(i)("Per").ToString()
                    .Cells(8).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                    .Cells(9).Value = Format(Val(dt.Rows(i)("Commamt").ToString()), "0.00")
                    .Cells(10).Value = Format(Val(dt.Rows(i)("Mamt").ToString()), "0.00")
                    .Cells(11).Value = Format(Val(dt.Rows(i)("rdfamt").ToString()), "0.00")
                    .Cells(12).Value = Format(Val(dt.Rows(i)("TareAmt").ToString()), "0.00")
                    .Cells(13).Value = Format(Val(dt.Rows(i)("labourAmt").ToString()), "0.00")
                    .Cells(14).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    .Cells(15).Value = If(Val(ClBal) >= 0, Format(Math.Abs(Val(ClBal)), "0.00") & " Dr", Format(Math.Abs(Val(ClBal)), "0.00") & " Cr")
                    .Cells(16).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                    If dt.Rows(i)("TransType").ToString() = "Standard Sale" Then
                        Dim OtherCHarges As Decimal = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalCharges) From Vouchers Where TransType='Standard Sale' and AccountID='" & Val(dt.Rows(i)("AccountID")) & "'")), "0.00")
                        .Cells(17).Value = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID=" & Val(dt.Rows(i)("AccountID")) & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'") + OtherCHarges), "0.00")
                    Else
                        .Cells(17).Value = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID=" & Val(dt.Rows(i)("AccountID")) & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    End If
                    .Cells(18).Value = HindiAccount 'clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & Val(dt.Rows(i)("AccountID").ToString()) & "")
                    .Cells(19).Value = HindiItem 'clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & Val(dt.Rows(i)("ItemID").ToString()) & "") & IIf(Val(dt.Rows(i)("CrateQty").ToString()) = 0, "", " (" & dt.Rows(i)("CrateMarka").ToString() & " : " & dt.Rows(i)("CrateQty").ToString() & ")")
                    .Cells(20).Value = AccountCreditAmt 'Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(21).Value = CashTotalAmount 'Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID='7' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(22).Value = CreditTotalAmount 'Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(23).Value = TotalAmount 'Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(24).Value = TotalNug 'Format(Val(clsFun.ExecScalarStr("Select sum(nug) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(25).Value = TotalWeight 'Format(Val(clsFun.ExecScalarStr("Select sum(Weight) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(26).Value = TotalComm 'Format(Val(clsFun.ExecScalarStr("Select sum(CommAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(27).Value = TotalMandiTax 'Format(Val(clsFun.ExecScalarStr("Select sum(MAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(28).Value = TotalRDF 'Format(Val(clsFun.ExecScalarStr("Select sum(RdfAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(29).Value = TotalTare 'Format(Val(clsFun.ExecScalarStr("Select sum(TareAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(30).Value = TotalLabour 'Format(Val(clsFun.ExecScalarStr("Select sum(Labour) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(31).Value = TotalAmount2 'Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(32).Value = TotalRoundOff 'Format(Val(clsFun.ExecScalarStr("Select sum(Roundoff) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(33).Value = CashAmount 'Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Transaction2 where  accountID='7' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(34).Value = CreditAMount ' Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Transaction2 where  accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(35).Value = CreditNug 'Format(Val(clsFun.ExecScalarStr("Select sum(nug) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(36).Value = CreditWeight 'Format(Val(clsFun.ExecScalarStr("Select sum(Weight) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(37).Value = CreditComm 'Format(Val(clsFun.ExecScalarStr("Select sum(CommAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(38).Value = CreditMamt 'Format(Val(clsFun.ExecScalarStr("Select sum(MAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(39).Value = CreditRDF 'Format(Val(clsFun.ExecScalarStr("Select sum(RdfAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(40).Value = CreditTare 'Format(Val(clsFun.ExecScalarStr("Select sum(TareAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(41).Value = CreDitLabour 'Format(Val(clsFun.ExecScalarStr("Select sum(Labour) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(42).Value = Format(Val(.Cells(22).Value) - Val(.Cells(40).Value), "0.00")
                    .Cells(43).Value = Format(Val(.Cells(14).Value) - Val(.Cells(12).Value), "0.00")
                    .Cells(44).Value = STDTotalCharges 'Format(Val(clsFun.ExecScalarStr("Select Sum(TotalCharges) From Vouchers Where TransType='Standard Sale' and AccountID='" & Val(dt.Rows(i)("AccountID")) & "'")), "0.00")
                    .Cells(45).Value = TotalChargesWithoutComm 'Format(Val(clsFun.ExecScalarStr("Select sum(Amount)+ sum(Commamt) + sum(MAmt)+ sum(RdfAmt)+ sum(LabourAmt)+sum(RoundOff) as tot from Transaction2 where CommAmt = 0 and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(46).Value = TotalChargesWithComm 'Format(Val(clsFun.ExecScalarStr("Select sum(Amount)+ sum(Commamt) + sum(MAmt)+ sum(RdfAmt)+ sum(LabourAmt)+sum(RoundOff) as tot from Transaction2 where CommAmt <> 0 and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(47).Value = AccountCrates 'Format(Val(clsFun.ExecScalarStr("Select sum(CrateQty) as CrateQty  from Transaction2 where AccountID='" & .Cells(0).Value & "' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")))
                    .Cells(48).Value = TotalCrates 'Format(Val(clsFun.ExecScalarStr("Select sum(CrateQty) as CrateQty  from Transaction2 where  EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")))
                    lastval = i + 1
                End With
            Next i
            dt.Clear()
            '''''Cash Record
            ssql = "Select *  from Transaction2 Where  AccountID in (Select ID From Accounts Where GroupID=11) and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and TransType Not In('On Sale','Store Transfer') order by AccountName "
            dt = clsFun.ExecDataTable(ssql)
            If dt.Rows.Count > 0 Then
                ' pnlWait.Visible = True
                '  Application.DoEvents()
                pb1.Minimum = lastval
                pb1.Maximum = lastval + dt.Rows.Count - 1
                For j = 0 To dt.Rows.Count - 1
                    'pnlWait.Visible = True

                    pb1.Value = pb1.Minimum + j
                    ''''''''''''''''''''' Opening Balance'''''''''''''''''''''''''''''''''''
                    opbal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                             "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                             " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                             " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(j)("AccountID")) & " Order by upper(AccountName) ;"))

                    ''''''''''''''''''''closing balance'''''''''''''''''''''''''

                    ClBal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                             "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                             " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                             " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(j)("AccountID")) & " Order by upper(AccountName) ;"))

                    Dim AccountCreditAmt As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    Dim HindiAccount As String = clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & Val(dt.Rows(j)("AccountID").ToString()) & "")
                    Dim HindiItem As String = clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & Val(dt.Rows(j)("ItemID").ToString()) & "") & IIf(Val(dt.Rows(j)("CrateQty").ToString()) = 0, "", " (" & dt.Rows(j)("CrateMarka").ToString() & " : " & dt.Rows(j)("CrateQty").ToString() & ")")
                    Dim STDTotalCharges As Decimal = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalCharges) From Vouchers Where TransType='Standard Sale' and AccountID='" & Val(dt.Rows(j)("AccountID")) & "'")), "0.00")
                    Dim AccountCrates As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(CrateQty) as CrateQty  from Transaction2 where AccountID='" & Val(dt.Rows(j)("AccountID")) & "' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")))

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    tmpgrid.Rows.Add()
                    With tmpgrid.Rows(lastval)
                        'If dt.Rows(j)("Id").ToString() = 878 Then MsgBox("A")
                        .Cells(0).Value = dt.Rows(j)("AccountID").ToString()
                        .Cells(1).Value = dt.Rows(j)("AccountName").ToString()
                        .Cells(2).Value = If(Val(opbal) >= 0, Format(Math.Abs(Val(opbal)), "0.00") & " Dr", Format(Math.Abs(Val(opbal)), "0.00") & " Cr")
                        .Cells(3).Value = dt.Rows(j)("ItemName").ToString() & IIf(Val(dt.Rows(j)("CrateQty").ToString()) = 0, "", " (" & dt.Rows(j)("CrateMarka").ToString() & " : " & dt.Rows(j)("CrateQty").ToString() & ")")
                        .Cells(4).Value = Format(Val(dt.Rows(j)("Nug").ToString()), "0.00")
                        .Cells(5).Value = Format(Val(dt.Rows(j)("Weight").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(j)("Rate").ToString()), "0.00")
                        .Cells(7).Value = dt.Rows(j)("Per").ToString()
                        .Cells(8).Value = Format(Val(dt.Rows(j)("Amount").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(j)("Commamt").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(j)("Mamt").ToString()), "0.00")
                        .Cells(11).Value = Format(Val(dt.Rows(j)("rdfamt").ToString()), "0.00")
                        .Cells(12).Value = Format(Val(dt.Rows(j)("TareAmt").ToString()), "0.00")
                        .Cells(13).Value = Format(Val(dt.Rows(j)("labourAmt").ToString()), "0.00")
                        .Cells(14).Value = Format(Val(dt.Rows(j)("TotalAmount").ToString()), "0.00")
                        .Cells(15).Value = If(Val(ClBal) >= 0, Format(Math.Abs(Val(ClBal)), "0.00") & " Dr", Format(Math.Abs(Val(ClBal)), "0.00") & " Cr")
                        .Cells(16).Value = CDate(dt.Rows(j)("EntryDate")).ToString("dd-MM-yyyy")
                        If dt.Rows(j)("TransType").ToString() = "Standard Sale" Then
                            Dim OtherCHarges As Decimal = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalCharges) From Vouchers Where TransType='Standard Sale' and AccountID='" & Val(dt.Rows(j)("AccountID")) & "'")), "0.00")
                            .Cells(17).Value = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID=" & Val(dt.Rows(j)("AccountID")) & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'") + OtherCHarges), "0.00")
                        Else
                            .Cells(17).Value = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID=" & Val(dt.Rows(j)("AccountID")) & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        End If
                        .Cells(18).Value = HindiAccount 'clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & Val(dt.Rows(j)("AccountID").ToString()) & "")
                        .Cells(19).Value = HindiItem 'clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & Val(dt.Rows(j)("ItemID").ToString()) & "") & IIf(Val(dt.Rows(j)("CrateQty").ToString()) = 0, "", " (" & dt.Rows(j)("CrateMarka").ToString() & " : " & dt.Rows(j)("CrateQty").ToString() & ")")
                        .Cells(20).Value = AccountCreditAmt 'Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(21).Value = CashTotalAmount 'Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID='7' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(22).Value = CreditTotalAmount 'Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(23).Value = TotalAmount 'Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(24).Value = TotalNug 'Format(Val(clsFun.ExecScalarStr("Select sum(nug) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(25).Value = TotalWeight 'Format(Val(clsFun.ExecScalarStr("Select sum(Weight) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(26).Value = TotalComm 'Format(Val(clsFun.ExecScalarStr("Select sum(CommAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(27).Value = TotalMandiTax 'Format(Val(clsFun.ExecScalarStr("Select sum(MAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(28).Value = TotalRDF 'Format(Val(clsFun.ExecScalarStr("Select sum(RdfAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(29).Value = TotalTare 'Format(Val(clsFun.ExecScalarStr("Select sum(TareAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(30).Value = TotalLabour 'Format(Val(clsFun.ExecScalarStr("Select sum(Labour) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(31).Value = TotalAmount2 'Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(32).Value = TotalRoundOff 'Format(Val(clsFun.ExecScalarStr("Select sum(Roundoff) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(33).Value = CashAmount 'Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Transaction2 where  accountID='7' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(34).Value = CreditAMount ' Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Transaction2 where  accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(35).Value = CreditNug 'Format(Val(clsFun.ExecScalarStr("Select sum(nug) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(36).Value = CreditWeight 'Format(Val(clsFun.ExecScalarStr("Select sum(Weight) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(37).Value = CreditComm 'Format(Val(clsFun.ExecScalarStr("Select sum(CommAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(38).Value = CreditMamt 'Format(Val(clsFun.ExecScalarStr("Select sum(MAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(39).Value = CreditRDF 'Format(Val(clsFun.ExecScalarStr("Select sum(RdfAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(40).Value = CreditTare 'Format(Val(clsFun.ExecScalarStr("Select sum(TareAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(41).Value = CreDitLabour 'Format(Val(clsFun.ExecScalarStr("Select sum(Labour) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(42).Value = Format(Val(.Cells(22).Value) - Val(.Cells(40).Value), "0.00")
                        .Cells(43).Value = Format(Val(.Cells(14).Value) - Val(.Cells(12).Value), "0.00")
                        .Cells(44).Value = STDTotalCharges 'Format(Val(clsFun.ExecScalarStr("Select Sum(TotalCharges) From Vouchers Where TransType='Standard Sale' and AccountID='" & Val(dt.Rows(j)("AccountID")) & "'")), "0.00")
                        .Cells(45).Value = TotalChargesWithoutComm 'Format(Val(clsFun.ExecScalarStr("Select sum(Amount)+ sum(Commamt) + sum(MAmt)+ sum(RdfAmt)+ sum(LabourAmt)+sum(RoundOff) as tot from Transaction2 where CommAmt = 0 and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(46).Value = TotalChargesWithComm 'Format(Val(clsFun.ExecScalarStr("Select sum(Amount)+ sum(Commamt) + sum(MAmt)+ sum(RdfAmt)+ sum(LabourAmt)+sum(RoundOff) as tot from Transaction2 where CommAmt <> 0 and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                        .Cells(47).Value = AccountCrates 'Format(Val(clsFun.ExecScalarStr("Select sum(CrateQty) as CrateQty  from Transaction2 where AccountID='" & .Cells(0).Value & "' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")))
                        .Cells(48).Value = TotalCrates 'Format(Val(clsFun.ExecScalarStr("Select sum(CrateQty) as CrateQty  from Transaction2 where  EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")))
                        lastval = lastval + 1
                    End With
                Next j
                dt.Clear()

            End If
        End If
        pnlWait.Visible = False
    End Sub
    Private Sub printDetailedWithOutCash()
        pnlWait.Visible = True
        Application.DoEvents()
        tmpgrid.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim opbal As String = ""
        Dim ClBal As String = ""
        Dim count As Integer = 0
        Dim ssql As String = String.Empty
        'ssql = "Select t2.EntryDate as entrydate, Ac.AccountName as AccountName,Ac.otherName as AccountHindiName,ac.id as id, i.othername as hname ,t2.Charges as Charges,t2.ItemName as ItemName,t2.Nug as nug,t2.Weight as Weight,t2.Rate as rate,t2.Per as per, t2.Amount as amount,t2.CommAmt as commamt,t2.MAmt as mamt,t2.RdfAmt as RdfAmt,t2.TareAmt as TareAmt,t2.labouramt as labour,  t2.TotalAmount as TotalAmount,t2.Cratemarka as Cratemarka,t2.CrateQty as CrateQty ,v.TransType as TransType    from Account_acgrp ac " & _
        '    "inner join Transaction2 t2 on ac.id=t2.accountid Inner join items i on i.id=t2.itemid left join vouchers v on v.id =t2.voucherid where (ac.groupid =16 or ac.undergroupid=16)  " & _
        '    "and  t2.EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'  order by ac.accountname "
        ssql = "Select *  from Transaction2 Where  AccountID in (Select ID From Accounts Where GroupID<>11) and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and TransType Not In('On Sale','Store Transfer') order by AccountName "

        Dim CashTotalAmount As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID='7' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditTotalAmount As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalAmount As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalNug As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(nug) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalWeight As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Weight) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalComm As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(CommAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalMandiTax As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(MAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalRDF As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(RdfAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalTare As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(TareAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalLabour As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Labour) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalAmount2 As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalRoundOff As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Roundoff) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CashAmount As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Transaction2 where  accountID='7' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditAMount As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Transaction2 where  accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditNug As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(nug) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditWeight As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Weight) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditComm As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(CommAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditMamt As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(MAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditRDF As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(RdfAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreditTare As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(TareAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim CreDitLabour = Format(Val(clsFun.ExecScalarStr("Select sum(Labour) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalChargesWithoutComm As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Amount)+ sum(Commamt) + sum(MAmt)+ sum(RdfAmt)+ sum(LabourAmt)+sum(RoundOff) as tot from Transaction2 where CommAmt = 0 and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalChargesWithComm As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Amount)+ sum(Commamt) + sum(MAmt)+ sum(RdfAmt)+ sum(LabourAmt)+sum(RoundOff) as tot from Transaction2 where CommAmt <> 0 and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
        Dim TotalCrates As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(CrateQty) as CrateQty  from Transaction2 where  EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")))

        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            ' pnlWait.Visible = True
            '  Application.DoEvents()
            pb1.Minimum = 0
            For i = 0 To dt.Rows.Count - 1
                'pnlWait.Visible = True
                pb1.Maximum = dt.Rows.Count - 1
                pb1.Value = i
                ''''''''''''''''''''' Opening Balance'''''''''''''''''''''''''''''''''''
                opbal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                         "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                         " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                         " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID")) & " Order by upper(AccountName) ;"))

                ''''''''''''''''''''closing balance'''''''''''''''''''''''''

                ClBal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                         "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
                                         " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
                                         " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID")) & " Order by upper(AccountName) ;"))

                Dim AccountCreditAmt As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                Dim HindiAccount As String = clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & Val(dt.Rows(i)("AccountID").ToString()) & "")
                Dim HindiItem As String = clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & Val(dt.Rows(i)("ItemID").ToString()) & "") & IIf(Val(dt.Rows(i)("CrateQty").ToString()) = 0, "", " (" & dt.Rows(i)("CrateMarka").ToString() & " : " & dt.Rows(i)("CrateQty").ToString() & ")")
                Dim STDTotalCharges As Decimal = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalCharges) From Vouchers Where TransType='Standard Sale' and AccountID='" & Val(dt.Rows(i)("AccountID")) & "'")), "0.00")
                Dim AccountCrates As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(CrateQty) as CrateQty  from Transaction2 where AccountID='" & Val(dt.Rows(i)("AccountID")) & "' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")))

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                tmpgrid.Rows.Add()
                With tmpgrid.Rows(i)
                    'If dt.Rows(i)("Id").ToString() = 878 Then MsgBox("A")
                    .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
                    .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(2).Value = If(Val(opbal) >= 0, Format(Math.Abs(Val(opbal)), "0.00") & " Dr", Format(Math.Abs(Val(opbal)), "0.00") & " Cr")
                    .Cells(3).Value = dt.Rows(i)("ItemName").ToString() & IIf(Val(dt.Rows(i)("CrateQty").ToString()) = 0, "", " (" & dt.Rows(i)("CrateMarka").ToString() & " : " & dt.Rows(i)("CrateQty").ToString() & ")")
                    .Cells(4).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                    .Cells(5).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                    .Cells(6).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                    .Cells(7).Value = dt.Rows(i)("Per").ToString()
                    .Cells(8).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                    .Cells(9).Value = Format(Val(dt.Rows(i)("Commamt").ToString()), "0.00")
                    .Cells(10).Value = Format(Val(dt.Rows(i)("Mamt").ToString()), "0.00")
                    .Cells(11).Value = Format(Val(dt.Rows(i)("rdfamt").ToString()), "0.00")
                    .Cells(12).Value = Format(Val(dt.Rows(i)("TareAmt").ToString()), "0.00")
                    .Cells(13).Value = Format(Val(dt.Rows(i)("labourAmt").ToString()), "0.00")
                    .Cells(14).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    .Cells(15).Value = If(Val(ClBal) >= 0, Format(Math.Abs(Val(ClBal)), "0.00") & " Dr", Format(Math.Abs(Val(ClBal)), "0.00") & " Cr")
                    .Cells(16).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                    If dt.Rows(i)("TransType").ToString() = "Standard Sale" Then
                        Dim OtherCHarges As Decimal = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalCharges) From Vouchers Where TransType='Standard Sale' and AccountID='" & Val(dt.Rows(i)("AccountID")) & "'")), "0.00")
                        .Cells(17).Value = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID=" & Val(dt.Rows(i)("AccountID")) & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'") + OtherCHarges), "0.00")
                    Else
                        .Cells(17).Value = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID=" & Val(dt.Rows(i)("AccountID")) & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    End If
                    .Cells(18).Value = HindiAccount 'clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & Val(dt.Rows(i)("AccountID").ToString()) & "")
                    .Cells(19).Value = HindiItem 'clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & Val(dt.Rows(i)("ItemID").ToString()) & "") & IIf(Val(dt.Rows(i)("CrateQty").ToString()) = 0, "", " (" & dt.Rows(i)("CrateMarka").ToString() & " : " & dt.Rows(i)("CrateQty").ToString() & ")")
                    .Cells(20).Value = AccountCreditAmt 'Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(21).Value = CashTotalAmount 'Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID='7' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(22).Value = CreditTotalAmount 'Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where  accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(23).Value = TotalAmount 'Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(24).Value = TotalNug 'Format(Val(clsFun.ExecScalarStr("Select sum(nug) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(25).Value = TotalWeight 'Format(Val(clsFun.ExecScalarStr("Select sum(Weight) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(26).Value = TotalComm 'Format(Val(clsFun.ExecScalarStr("Select sum(CommAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(27).Value = TotalMandiTax 'Format(Val(clsFun.ExecScalarStr("Select sum(MAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(28).Value = TotalRDF 'Format(Val(clsFun.ExecScalarStr("Select sum(RdfAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(29).Value = TotalTare 'Format(Val(clsFun.ExecScalarStr("Select sum(TareAmt) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(30).Value = TotalLabour 'Format(Val(clsFun.ExecScalarStr("Select sum(Labour) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(31).Value = TotalAmount2 'Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(32).Value = TotalRoundOff 'Format(Val(clsFun.ExecScalarStr("Select sum(Roundoff) as tot from Transaction2 where EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(33).Value = CashAmount 'Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Transaction2 where  accountID='7' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(34).Value = CreditAMount ' Format(Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Transaction2 where  accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(35).Value = CreditNug 'Format(Val(clsFun.ExecScalarStr("Select sum(nug) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(36).Value = CreditWeight 'Format(Val(clsFun.ExecScalarStr("Select sum(Weight) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(37).Value = CreditComm 'Format(Val(clsFun.ExecScalarStr("Select sum(CommAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(38).Value = CreditMamt 'Format(Val(clsFun.ExecScalarStr("Select sum(MAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(39).Value = CreditRDF 'Format(Val(clsFun.ExecScalarStr("Select sum(RdfAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(40).Value = CreditTare 'Format(Val(clsFun.ExecScalarStr("Select sum(TareAmt) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(41).Value = CreDitLabour 'Format(Val(clsFun.ExecScalarStr("Select sum(Labour) as tot from Transaction2 where accountID not in ('7') and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(42).Value = Format(Val(.Cells(22).Value) - Val(.Cells(40).Value), "0.00")
                    .Cells(43).Value = Format(Val(.Cells(14).Value) - Val(.Cells(12).Value), "0.00")
                    .Cells(44).Value = STDTotalCharges 'Format(Val(clsFun.ExecScalarStr("Select Sum(TotalCharges) From Vouchers Where TransType='Standard Sale' and AccountID='" & Val(dt.Rows(i)("AccountID")) & "'")), "0.00")
                    .Cells(45).Value = TotalChargesWithoutComm 'Format(Val(clsFun.ExecScalarStr("Select sum(Amount)+ sum(Commamt) + sum(MAmt)+ sum(RdfAmt)+ sum(LabourAmt)+sum(RoundOff) as tot from Transaction2 where CommAmt = 0 and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(46).Value = TotalChargesWithComm 'Format(Val(clsFun.ExecScalarStr("Select sum(Amount)+ sum(Commamt) + sum(MAmt)+ sum(RdfAmt)+ sum(LabourAmt)+sum(RoundOff) as tot from Transaction2 where CommAmt <> 0 and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(47).Value = AccountCrates 'Format(Val(clsFun.ExecScalarStr("Select sum(CrateQty) as CrateQty  from Transaction2 where AccountID='" & .Cells(0).Value & "' and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")))
                    .Cells(48).Value = TotalCrates 'Format(Val(clsFun.ExecScalarStr("Select sum(CrateQty) as CrateQty  from Transaction2 where  EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")))
                    lastval = i + 1
                End With
            Next i
            dt.Clear()
        End If
        pnlWait.Visible = False
    End Sub
 

    Private Sub ckCashBankBills_CheckedChanged(sender As Object, e As EventArgs) Handles ckCashBankBills.CheckedChanged

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If dg1.RowCount = 0 Then
            MsgBox("There is No record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            pnlWait.Visible = True
            ButtonControl()
            IDGentateDay2Day()
            ' PrintSlips()
            PrintDay2Day()
            ButtonControl()
            pnlWait.Visible = False
            Report_Viewer.printReport("\Day2day.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub
    Private Sub IDGentateDay2Day()
        Dim id As String = String.Empty
        Dim checkBox As DataGridViewCheckBoxCell
        For Each row As DataGridViewRow In dgAccounts.Rows
            checkBox = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            If checkBox.Value = True Then
                id = id & row.Cells(1).Value & ","
            End If
        Next
        If id = "" Then
            '  retrive(id)
            Day2Day(id)
            Exit Sub
        Else
            id = id.Remove(id.LastIndexOf(","))
            'retrive(id)
            Day2Day(id)
        End If
    End Sub

  
    Private Sub PrintDay2Day()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        ' clsFun.ExecNonQuery(sql)
        For Each row As DataGridViewRow In tmpgrid.Rows
            Application.DoEvents()
            pb1.Minimum = 0
            pb1.Maximum = tmpgrid.Rows.Count
            With row
                pb1.Value = row.Index
                If .Cells(2).Value <> "" Then
                    sql = sql & "insert into Printing(D1,D2,P1, P2,P3, P4, P5, P6,P7,P8,P9, " &
                        " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " &
                        " P21, P22,P23, P24, P25, P26,P27,P28,P29, " &
                        " P30,P31,P32,P33,P34,P35,P36,P37,P38,P39,P40,P41,P42,P43,P44,P45,P46,M3,P47,P48)" &
                        "  values('" & mskFromDate.Text & "','" & MsktoDate.Text & "','" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," &
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " &
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " &
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " &
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " &
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "'," &
                                "'" & .Cells(25).Value & "','" & .Cells(26).Value & "','" & .Cells(27).Value & "','" & .Cells(28).Value & "', " &
                                "'" & .Cells(29).Value & "','" & .Cells(30).Value & "','" & .Cells(31).Value & "','" & .Cells(32).Value & "', " &
                                "'" & .Cells(33).Value & "','" & .Cells(34).Value & "','" & .Cells(35).Value & "','" & .Cells(36).Value & "'," &
                                "'" & .Cells(37).Value & "','" & .Cells(38).Value & "','" & .Cells(39).Value & "','" & .Cells(40).Value & "'," &
                                "'" & .Cells(41).Value & "','" & .Cells(42).Value & "','" & .Cells(43).Value & "','" & .Cells(44).Value & "'," &
                                "'" & .Cells(45).Value & "','" & .Cells(46).Value & "','" & .Cells(47).Value & "','" & .Cells(48).Value & "','" & .Cells(49).Value & "');"
                End If
            End With
        Next
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
            If cmd.ExecuteNonQuery() > 0 Then count = +1
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
        pnlWait.Visible = False
        ClsFunPrimary.CloseConnection()
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        If isBackgroundWorkerRunning Then
            ' e.Cancel = True
            Me.Hide()
            ' MessageBox.Show("The process is still running. The form will be hidden instead of closed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Top = 0 : Me.Left = 0
        Else
            Dim msgRslt As MsgBoxResult = MsgBox("Are you Sure Want to Close Entry?", MsgBoxStyle.YesNo, "Aadhat")
            If msgRslt = MsgBoxResult.Yes Then
                Me.Close() : Exit Sub
            ElseIf msgRslt = MsgBoxResult.No Then
            End If
        End If
    End Sub
    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim id As Integer = Val(dg1.SelectedRows(0).Cells(0).Value)
        Dim type As String = dg1.SelectedRows(0).Cells(11).Value
        If type = "Stock Sale" Then
            Stock_Sale.MdiParent = MainScreenForm
            Stock_Sale.Show()
            Stock_Sale.FillControls(id)
            Stock_Sale.BringToFront()
            Stock_Sale.mskEntryDate.SelectAll()
        ElseIf type = "Super Sale" Then
            Super_Sale.MdiParent = MainScreenForm
            Super_Sale.Show()
            Super_Sale.FillControls(id)
            Super_Sale.BringToFront()
            Super_Sale.mskEntryDate.SelectAll()
        ElseIf type = "Speed Sale" Then
            SpeedSale.MdiParent = MainScreenForm
            SpeedSale.Show()
            SpeedSale.FillContros(id)
            SpeedSale.BringToFront()
            SpeedSale.mskEntryDate.SelectAll()
        End If
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            Dim id As Integer = dg1.SelectedRows(0).Cells(0).Value
            Dim type As String = dg1.SelectedRows(0).Cells(11).Value
            If type = "Stock Sale" Then
                Stock_Sale.MdiParent = MainScreenForm
                Stock_Sale.Show()
                Stock_Sale.FillControls(id)
                Stock_Sale.BringToFront()
                Stock_Sale.mskEntryDate.SelectAll()
            ElseIf type = "Super Sale" Then
                Super_Sale.MdiParent = MainScreenForm
                Super_Sale.Show()
                Super_Sale.FillControls(id)
                Super_Sale.BringToFront()
                Super_Sale.mskEntryDate.SelectAll()
            ElseIf type = "Speed Sale" Then
                SpeedSale.MdiParent = MainScreenForm
                SpeedSale.Show()
                SpeedSale.FillContros(id)
                SpeedSale.BringToFront()
                SpeedSale.mskEntryDate.SelectAll()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles dtp2.GotFocus
        MsktoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        MsktoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
    Public Sub FillControl()
        Dim SendingMethod As String
        Dim LangugageType As String
        Dim MsgType As String
        Dim Sql As String = "Select * From API"
        Dim dt As New DataTable
        dt = ClsFunPrimary.ExecDataTable(Sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    SendingMethod = dt.Rows(i)("SendingMethod").ToString()
                    cbType.SelectedIndex = 0
                    If SendingMethod = "Easy WhatsApp" Then cbType.SelectedIndex = 0 Else cbType.SelectedIndex = 0 : cbType.Visible = True
                    LangugageType = dt.Rows(i)("LanguageType").ToString()
                    btnRadioEnglish.Checked = True
                    If LangugageType = "English" Then btnRadioEnglish.Checked = True Else RadioRegional.Checked = True
                    MsgType = dt.Rows(i)("SendingType").ToString()
                    RadioPDFMsg.Checked = True
                    If MsgType = "Pdf + Msg" Then RadioPDFMsg.Checked = True
                    If MsgType = "Pdf Only" Then RadioPdfOnly.Checked = True
                    If MsgType = "Msg Only" Then RadioMsgOnly.Checked = True
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        'clsFun.CloseConnection()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles BtnSendWhatsapp.Click
        If CDate(mskFromDate.Text).ToString("dd-MM-yyyy") <> CDate(MsktoDate.Text).ToString("dd-MM-yyyy") Then
            MessageBox.Show("You can WhatsApp by one date only.", "Date Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error)
            mskFromDate.Focus()
            Exit Sub
        End If
        FillControl()
        If ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "Easy WhatsApp" Then
            If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp() : ShowWhatsappContacts()
            pnlWhatsapp.Visible = True
            Dim WhatsappFile As String = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
            If System.IO.File.Exists(WhatsappFile) = False Then
                MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
                Exit Sub
            End If
            Dim p() As Process
            p = Process.GetProcessesByName("Easy Whatsapp")
            If p.Count = 0 Then
                Dim StartWhatsapp As New System.Diagnostics.Process
                StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
                StartWhatsapp.Start()
            End If
            cbType.SelectedIndex = 0 : Exit Sub
        ElseIf ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "WahSoft" Then
            cbType.SelectedIndex = 1
        End If
        pnlWhatsapp.Visible = True
        If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp()
        ShowWhatsappContacts()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnSending.Click
        If cbType.SelectedIndex = 0 Then
            Dim WhatsappFile As String = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
            If System.IO.File.Exists(WhatsappFile) = False Then
                MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
                Exit Sub
            End If
            Dim p() As Process
            p = Process.GetProcessesByName("Easy Whatsapp")
            If p.Count = 0 Then
                Dim StartWhatsapp As New System.Diagnostics.Process
                StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
                StartWhatsapp.Start()
            End If
            SendWhatsApp()
        End If
    End Sub

    Private Sub StartBackgroundTask(action As Action)
        If Not bgWorker.IsBusy Then
            bgWorker.RunWorkerAsync(action)
            '    MsgBox("A background task is running. you can Use your Task", MsgBoxStyle.Information, "Background Task")
        Else
            MsgBox("A background task is already running.", MsgBoxStyle.Information, "Background Task")
        End If
    End Sub
    Private Sub bgWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        isBackgroundWorkerRunning = True
        Dim action As Action = CType(e.Argument, Action)
        action.Invoke()
    End Sub

    Private Sub bgWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        isBackgroundWorkerRunning = False
    End Sub

    Private Sub UpdateProgressBar(value As Integer)
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(New Action(Of Integer)(AddressOf UpdateProgressBar), value)
        Else
            If value <= ProgressBar1.Maximum Then
                ProgressBar1.Value = value
            End If
        End If
    End Sub

    Private Sub UpdateProgressBarVisibility(visible As Boolean)
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(New Action(Of Boolean)(AddressOf UpdateProgressBarVisibility), visible)
        Else
            ProgressBar1.Visible = visible
        End If
    End Sub

    Private Sub btnPnlVisHide_Click(sender As Object, e As EventArgs) Handles btnPnlVisHide.Click
        Dim pProcess() As Process = System.Diagnostics.Process.GetProcessesByName("Easy Whatsapp")
        For Each p As Process In pProcess
            p.Kill()
        Next
        pnlWhatsapp.Visible = False
    End Sub

    Private Sub DgWhatsapp_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DgWhatsapp.CellEndEdit
        If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & DgWhatsapp.CurrentRow.Cells(1).Value & "'") = "" And DgWhatsapp.CurrentRow.Cells(3).Value <> "" Then
            clsFun.ExecScalarStr("Update Accounts set Mobile1='" & DgWhatsapp.CurrentRow.Cells(3).Value & "' Where ID='" & Val(DgWhatsapp.CurrentRow.Cells(1).Value) & "'")
        Else
            If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & DgWhatsapp.CurrentRow.Cells(1).Value & "'") <> DgWhatsapp.CurrentRow.Cells(3).Value Then
                If MessageBox.Show("Are you Sure to Change Mobile No in PhoneBook", "Change Number", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    clsFun.ExecScalarStr("Update Accounts set Mobile1='" & DgWhatsapp.CurrentRow.Cells(3).Value & "' Where ID='" & Val(DgWhatsapp.CurrentRow.Cells(1).Value) & "'")
                End If
            End If
        End If
    End Sub


End Class