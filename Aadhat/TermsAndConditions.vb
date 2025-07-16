Imports System.IO
Public Class TermsAndConditions
    Private Sub SaveToLicenseAgreementFile()
        Try
            ' Get the application's startup folder where the executable is located
            Dim startupFolder As String = Application.StartupPath

            ' Combine the folder path with the filename
            Dim filePath As String = Path.Combine(startupFolder, "LicenseAgreement.txt")

            ' Get the text from the RichTextBox control
            Dim agreementText As String = RichTextBox1.Text

            ' Write the text to the file
            File.WriteAllText(filePath, agreementText)

            '        MessageBox.Show("License agreement saved successfully.")
        Catch ex As Exception
            MessageBox.Show("Error saving license agreement: " & ex.Message)
        End Try
    End Sub


    Private Sub TermsAndConditions_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub TermsAndConditions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True : DisplayTNC() : btnAgree.Focus()
    End Sub
    Private Sub DisplayTNC()
        ' Hindi aur English mein Terms and Conditions likhna
        'RichTextBox1.AppendText("Terms and Conditions (नियम और शर्तें)" & vbCrLf & vbCrLf)
        RichTextBox1.SelectionColor = Color.Red
        RichTextBox1.SelectionFont = New Font("Arial Unicode MS", 12, FontStyle.Bold)
        RichTextBox1.AppendText("SOFT MANAGEMENT (Aadhat) LICENSE AGREEMENT" & vbCrLf & vbCrLf)
        RichTextBox1.SelectionColor = RichTextBox1.ForeColor ' Reset text color
        RichTextBox1.SelectionFont = RichTextBox1.Font ' 
        RichTextBox1.AppendText("By using this accounting software software developed by M/s SOFT MANAGEMENT(" & "LICENSOR" & ") (लेखा सॉफ़्टवेयर का उपयोग करके जिसके निर्माता M/s SOFT MANAGEMENT)," & vbCrLf & " you agree to the following terms and conditions (आप निम्नलिखित नियम और शर्तों से सहमति देते हैं):" & vbCrLf & vbCrLf)
        RichTextBox1.SelectionFont = New Font("Arial Unicode MS", 10, FontStyle.Bold)
        RichTextBox1.AppendText("Data Loss Disclaimer (डेटा हानि की अस्वीकृति):" & vbCrLf)
        RichTextBox1.AppendText("We do not guarantee the safety or integrity of your data. (हम आपके डेटा की सुरक्षा या पूर्णता की गारंटी नहीं देते हैं।)." & vbCrLf)
        RichTextBox1.AppendText("You are responsible for regularly backing up your data (आपकी जिम्मेदारी है कि आप अपने डेटा का नियमित बैकअप बनाएं।)." & vbCrLf)
        RichTextBox1.AppendText("In case of data loss, we will not be held liable (डेटा हानि के मामले में, हम जिम्मेदार नहीं होंगे)." & vbCrLf & vbCrLf)
        RichTextBox1.SelectionFont = New Font("Arial Unicode MS", 10, FontStyle.Bold)
        RichTextBox1.AppendText("No Source Code Provided (स्रोत कोड नहीं प्रदान किया जाएगा):" & vbCrLf)
        RichTextBox1.AppendText("We do not provide access to the source code of this software (हम इस सॉफ़्टवेयर के स्रोत कोड तक पहुँच प्रदान नहीं करते हैं।)." & vbCrLf)
        RichTextBox1.AppendText("You are not allowed to reverse engineer, modify, or distribute the software (आपको सॉफ़्टवेयर के साथ छेड़छाड़, संशोधित करने, या वितरित करने की अनुमति नहीं है।)." & vbCrLf & vbCrLf)
        RichTextBox1.SelectionFont = New Font("Arial Unicode MS", 10, FontStyle.Bold)
        RichTextBox1.AppendText("Updates and Changes (अपडेट और परिवर्तन):" & vbCrLf)
        RichTextBox1.AppendText("We reserve the right to make updates and changes to the software without prior notice (हमे पूर्व सूचना के बिना सॉफ़्टवेयर में अपडेट और परिवर्तन करने का अधिकार सुरक्षित रहता है।)." & vbCrLf)
        RichTextBox1.AppendText("These updates may affect the functionality and features of the software (इन अपडेट्स से सॉफ़्टवेयर की कार्यक्षमता और विशेषताएँ प्रभावित हो सकती हैं।)" & vbCrLf & vbCrLf)
        RichTextBox1.SelectionFont = New Font("Arial Unicode MS", 10, FontStyle.Bold)
        RichTextBox1.AppendText("Please read these terms and conditions carefully before using the accounting software (कृपया लेखा सॉफ़्टवेयर का उपयोग करने से पहले इन नियमों और शर्तों को ध्यान से पढ़ें...). If you do not agree with any of these terms, do not use the software (अगर आपको इनमें से कोई भी नियम पसंद नहीं है, तो सॉफ़्टवेयर का उपयोग न करें।)." & vbCrLf & vbCrLf)
        RichTextBox1.SelectionFont = New Font("Arial Unicode MS", 10, FontStyle.Bold)
        RichTextBox1.AppendText("These terms and conditions are subject to change without notice (इन नियमों और शर्तों को बिना सूचना के बदला जा सकता है।). It is your responsibility to review them periodically (यह आपकी जिम्मेदारी है कि आप नियमों की नियमित रूप से समीक्षा करें।)." & vbCrLf & vbCrLf)
        RichTextBox1.SelectionFont = New Font("Arial Unicode MS", 10, FontStyle.Underline)
        RichTextBox1.AppendText("Soft management will not be responsible for any kind of financial profit or loss in the customer's (Aadhat Software user) business.(सॉफ्ट मैनेजमेंट, ग्राहक (AADHAT USER) के व्यवसाय में किसी भी प्रकार के वित्तीय लाभ या हानि का जिम्मेदार नहीं होगा ।)" & vbCrLf & vbCrLf)
        RichTextBox1.AppendText("By using the accounting software, you acknowledge and accept these terms and conditions in their entirety (लेखा सॉफ़्टवेयर का उपयोग करके, आप इन नियमों और शर्तों को पूर्णतः स्वीकार करते हैं और उन्हें मान्यता देते हैं।)" & vbCrLf)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnAgree.Click
        Me.Hide() : SaveToLicenseAgreementFile()
        Create_Company.MdiParent = ShowCompanies
        Create_Company.Show()
        Create_Company.BringToFront()
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        MsgBox("Bye-Bye,See you Later", MsgBoxStyle.Information, "Good Bye")
        Application.Exit()
    End Sub
End Class