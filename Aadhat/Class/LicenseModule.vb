Public Module LicenseAmcModule

    Dim ClsCommon As New CommonClass()

    '===============================================
    '   FIRST LICENSE ACTIVATION
    '===============================================
    Public Sub ApplyLicense(licKey As String, firm As String, city As String, state As String,
                            mob1 As String, mob2 As String, email As String, address As String)

        Dim d As New LicenseData With {
            .license_key = licKey,
            .product_id = 1,
            .board_ids = ClsCommon.MotherboardSerialNumber(),
            .pc_name = Environment.MachineName,
            .firm_name = firm,
            .city = city,
            .state = state,
            .mobile1 = mob1,
            .mobile2 = mob2,
            .email = email,
            .address = address
        }

        Dim res = AccentStorageHelper.SaveLicense(d)
        Dim r = Newtonsoft.Json.JsonConvert.DeserializeObject(Of CustomerActivationResponse)(res)

        MsgBox(r.message, IIf(r.status = "success", vbInformation, vbCritical))
    End Sub


    '===============================================
    '   AMC ACTIVATION
    '===============================================
    Public Sub ActivateAmc(custId As String, licKey As String)

        Dim d As New AmcData With {
            .customer_code = custId,
            .license_key = licKey,
            .product_id = 1,
            .board_id = ClsCommon.MotherboardSerialNumber()
        }

        Dim res = AccentStorageHelper.SaveAmc(d)
        Dim r = Newtonsoft.Json.JsonConvert.DeserializeObject(Of AmcActivationResponse)(res)

        MsgBox(r.message, IIf(r.status = "success", vbInformation, vbCritical))
    End Sub

End Module
