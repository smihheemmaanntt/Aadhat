Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq
Imports System.Windows.Forms

Module PhoneMSg

    Private BASE_URL As String = "http://msgz.in"
    Private UPLOAD_KEY As String = "SOFTMANAGE@2026"
    Private _simMapLoaded As Boolean = False
    Private _slotToSubscriberId As New Dictionary(Of Integer, Integer)
    Private _slotLabels As New Dictionary(Of Integer, String)
    Private _subscriberIdLabels As New Dictionary(Of Integer, String)

    Private Function FirstJsonString(ByVal obj As JObject, ParamArray keys() As String) As String
        For Each key As String In keys
            Dim value As JToken = obj.SelectToken(key)
            If value IsNot Nothing AndAlso value.ToString().Trim() <> "" Then
                Return value.ToString().Trim()
            End If
        Next
        Return ""
    End Function

    Private Function FirstJsonInt(ByVal obj As JObject, ParamArray keys() As String) As Integer
        Return Val(FirstJsonString(obj, keys))
    End Function

    ' ==========================================
    ' COMMON POST JSON FUNCTION
    ' ==========================================
    Private Function PostJSON(ByVal url As String,
                              ByVal json As String,
                              ByVal token As String) As String

        Try
            Dim req As HttpWebRequest =
                CType(WebRequest.Create(url), HttpWebRequest)

            req.Method = "POST"
            req.ContentType = "application/json"
            req.Accept = "application/json"

            If token <> "" Then
                req.Headers.Add("Authorization", "Bearer " & token)
            End If

            Dim bytes() As Byte =
                Encoding.UTF8.GetBytes(json)

            req.ContentLength = bytes.Length

            Using stream As Stream = req.GetRequestStream()
                stream.Write(bytes, 0, bytes.Length)
            End Using

            Dim response As HttpWebResponse =
                CType(req.GetResponse(), HttpWebResponse)

            Using respStream As Stream =
                response.GetResponseStream()

                Using reader As New StreamReader(respStream)
                    Return reader.ReadToEnd()
                End Using

            End Using

        Catch ex As WebException
            Try
                If ex.Response IsNot Nothing Then
                    Using errResp As HttpWebResponse = CType(ex.Response, HttpWebResponse)
                        Using errStream As Stream = errResp.GetResponseStream()
                            Using errReader As New StreamReader(errStream)
                                Return "HTTP " & CInt(errResp.StatusCode).ToString() & " - " & errReader.ReadToEnd()
                            End Using
                        End Using
                    End Using
                End If
            Catch
            End Try
            Return ex.Message
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    Private Function GetToken() As String
        Return ClsFunPrimary.ExecScalarStr("Select Msg_Access_Token From API").Trim()
    End Function

    ' ==========================================
    ' SIM MAP LOAD FROM API
    ' ==========================================
    Public Function LoadSimMap(Optional ByVal accessToken As String = "") As Boolean
        Try
            _slotToSubscriberId.Clear()
            _slotLabels.Clear()
            _subscriberIdLabels.Clear()

            Dim token As String = accessToken.Trim()
            If token = "" Then token = GetToken()
            If token = "" Then
                _simMapLoaded = False
                Return False
            End If

            Dim url As String = BASE_URL & "/api/get/devices"
            Dim req As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            req.Method = "GET"
            req.ContentType = "application/json"
            req.Accept = "application/json"
            req.Headers.Add("Authorization", "Bearer " & token)

            Dim json As String = ""
            Using response As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
                Using respStream As Stream = response.GetResponseStream()
                    Using reader As New StreamReader(respStream)
                        json = reader.ReadToEnd()
                    End Using
                End Using
            End Using

            Dim simEntries As New List(Of String)
            Dim root As JObject = JObject.Parse(json)
            Dim dataArr As JArray = TryCast(root("data"), JArray)
            If dataArr IsNot Nothing AndAlso dataArr.Count > 0 Then
                Dim simToken As JToken = dataArr(0)("sim_info")
                If simToken IsNot Nothing Then
                    Dim simInfoText As String = simToken.ToString()
                    If simInfoText.Trim() <> "" Then
                        Dim simArr As JArray = JArray.Parse(simInfoText)
                        For Each item As JToken In simArr
                            simEntries.Add(item.ToString())
                        Next
                    End If
                End If
            End If

            If simEntries.Count = 0 Then
                _simMapLoaded = False
                Return False
            End If

            For Each simEntry As String In simEntries
                Dim slotZero As Integer = 0
                Dim subId As Integer = 0
                Dim opName As String = ""

                If simEntry.Trim().StartsWith("{") Then
                    Dim simObj As JObject = JObject.Parse(simEntry)
                    slotZero = FirstJsonInt(simObj, "slot", "sim_slot", "slot_index", "slotIndex")
                    subId = FirstJsonInt(simObj, "subscriber_id", "subscriberId", "subscription_id", "subscriptionId", "sub_id", "subId")
                    opName = FirstJsonString(simObj, "name", "sim_name", "simName", "operator", "operator_name", "operatorName", "carrier", "carrier_name", "carrierName", "display_name", "displayName", "network_name", "networkName")
                Else
                    Dim parts() As String = simEntry.Split(":"c)
                    If parts.Length < 2 Then Continue For
                    Integer.TryParse(parts(0), slotZero)
                    Integer.TryParse(parts(1), subId)
                    If parts.Length >= 3 Then opName = String.Join(":", parts, 2, parts.Length - 2).Trim()
                End If

                Dim slotDisplay As Integer = slotZero + 1
                If slotDisplay <= 0 OrElse subId <= 0 Then Continue For

                _slotToSubscriberId(slotDisplay) = subId

                Dim lbl As String = "SIM " & slotDisplay.ToString() & " (" & subId.ToString() & ")"
                If opName <> "" Then lbl &= " - " & opName
                _slotLabels(slotDisplay) = lbl
                _subscriberIdLabels(subId) = lbl
            Next

            _simMapLoaded = (_slotToSubscriberId.Count > 0)
            Return _simMapLoaded

        Catch
            _simMapLoaded = False
            Return False
        End Try
    End Function

    Public Function GetSimDisplayList(Optional ByVal accessToken As String = "", Optional ByVal includeFallback As Boolean = True) As List(Of String)
        Dim out As New List(Of String)

        If accessToken.Trim() <> "" OrElse _simMapLoaded = False Then
            LoadSimMap(accessToken)
        End If

        If _slotLabels.Count > 0 Then
            For slot As Integer = 1 To 4
                If _slotLabels.ContainsKey(slot) Then
                    out.Add(_slotLabels(slot))
                End If
            Next
        End If

        If out.Count = 0 AndAlso includeFallback Then
            out.Add("SIM 1")
            out.Add("SIM 2")
        End If

        Return out
    End Function

    Public Function ResolveSubscriberId(ByVal selectedSlot As Integer) As Integer
        If _simMapLoaded = False Then
            LoadSimMap()
        End If

        If _slotToSubscriberId.ContainsKey(selectedSlot) Then
            Return _slotToSubscriberId(selectedSlot)
        End If
        Return selectedSlot
    End Function

    Public Function ExtractSubscriberId(ByVal simText As String) As Integer
        If simText Is Nothing Then Return 0

        Dim m As Match = Regex.Match(simText, "\((\d+)\)")
        If m.Success Then
            Return Val(m.Groups(1).Value)
        End If

        Dim numericValue As Integer = Val(simText)
        If numericValue > 0 Then Return numericValue

        Return 0
    End Function

    Public Function GetSimDisplayText(ByVal subscriberId As String) As String
        If _simMapLoaded = False Then LoadSimMap()

        Dim subId As Integer = Val(subscriberId)
        If subId > 0 AndAlso _subscriberIdLabels.ContainsKey(subId) Then
            Return _subscriberIdLabels(subId)
        End If

        If subId > 0 Then Return "SIM (" & subId.ToString() & ")"
        Return ""
    End Function

    Public Function FindSimIndexBySubscriberId(ByVal subscriberId As String, ByVal comboItems As ComboBox.ObjectCollection) As Integer
        Dim subId As Integer = ExtractSubscriberId(subscriberId)
        If subId <= 0 Then Return -1

        For i As Integer = 0 To comboItems.Count - 1
            If ExtractSubscriberId(comboItems(i).ToString()) = subId Then
                Return i
            End If
        Next

        Return -1
    End Function

    ' ==========================================
    ' GET DEVICE ID FROM API
    ' ==========================================
    Public Function GetDeviceID() As String

        Try
            Dim token As String = GetToken()

            Dim url As String =
                BASE_URL & "/api/get/devices"

            Dim req As HttpWebRequest =
                CType(WebRequest.Create(url), HttpWebRequest)

            req.Method = "GET"
            req.ContentType = "application/json"
            req.Accept = "application/json"
            req.Headers.Add("Authorization", "Bearer " & token)

            Dim response As HttpWebResponse =
                CType(req.GetResponse(), HttpWebResponse)

            Using respStream As Stream =
                response.GetResponseStream()

                Using reader As New StreamReader(respStream)

                    Dim json As String =
                        reader.ReadToEnd()

                    Dim key As String = """id"":"
                    Dim i As Integer = json.IndexOf(key)

                    If i = -1 Then
                        Return "DEVICE ID NOT FOUND"
                    End If

                    Dim startPos As Integer = i + key.Length
                    Dim endPos As Integer = json.IndexOf(",", startPos)

                    If endPos = -1 Then
                        endPos = json.IndexOf("}", startPos)
                    End If

                    Dim deviceID As String =
                        json.Substring(startPos,
                                       endPos - startPos).Trim()

                    Return deviceID

                End Using

            End Using

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    Private Function GetSubscriberIdFromDevice(ByVal deviceId As String, ByVal token As String) As Integer
        Try
            If deviceId.Trim() = "" OrElse token.Trim() = "" Then Return 0

            Dim url As String = BASE_URL & "/api/get/subscriber/id?device_id=" & Uri.EscapeDataString(deviceId.Trim())
            Dim req As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            req.Method = "GET"
            req.ContentType = "application/json"
            req.Accept = "application/json"
            req.Headers.Add("Authorization", "Bearer " & token)

            Dim json As String = ""
            Using response As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
                Using respStream As Stream = response.GetResponseStream()
                    Using reader As New StreamReader(respStream)
                        json = reader.ReadToEnd()
                    End Using
                End Using
            End Using

            Dim root As JObject = JObject.Parse(json)
            Dim dataArr As JArray = TryCast(root("data"), JArray)
            If dataArr IsNot Nothing AndAlso dataArr.Count > 0 Then
                Return Val(dataArr(0).ToString())
            End If

        Catch
        End Try

        Return 0
    End Function

    ' ==========================================
    ' SEND MESSAGE (AUTO DEVICE ID)
    ' selected_sim can be "SIM 1 (2) - Jio", "2", or old slot no.
    ' ==========================================
    Public Function SendPhoneMsg(
        ByVal to_numbers As String,
        ByVal selected_sim As String,
        ByVal body As String
    ) As String

        Try
            Dim device_id As String = GetDeviceID()
            If device_id = "" OrElse device_id.Contains("NOT") Then
                Return device_id
            End If

            Dim token As String = GetToken()
            Dim subscriber_id As Integer = ExtractSubscriberId(selected_sim)
            If subscriber_id <= 0 Then
                subscriber_id = GetSubscriberIdFromDevice(device_id, token)
            End If

            If subscriber_id <= 0 Then
                Dim slotNo As Integer = Val(selected_sim)
                If slotNo <= 0 Then slotNo = 1
                subscriber_id = ResolveSubscriberId(slotNo)
            End If

            body = body.Replace("""", "\""") _
                       .Replace(vbCrLf, "\n")

            Dim json As String =
                "{""device_id"":""" & device_id &
                """,""to_numbers"":""" & to_numbers &
                """,""subscriber_id"":""" & subscriber_id.ToString() &
                """,""body"":""" & body & """}"

            Dim result As String =
                PostJSON(BASE_URL &
                         "/api/sent/compose",
                         json,
                         token)

            If result.Contains("Message queued successfully") Then
                Return "SUCCESS"
            Else
                Return result
            End If

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    ' ==========================================
    ' PDF UPLOAD (OPTIONAL)
    ' ==========================================
    Public Function UploadPDF_Local(
        ByVal filePath As String
    ) As String

        Try
            If Not File.Exists(filePath) Then
                Return "File Not Found"
            End If

            ServicePointManager.Expect100Continue = False
            ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType) Or SecurityProtocolType.Tls

            Dim boundary As String = "----AccoBookUpload" & DateTime.Now.Ticks.ToString()
            Dim uploadUrl As String = BASE_URL.TrimEnd("/"c) & "/upload.php"
            Dim fileName As String = Path.GetFileName(filePath)
            Dim fileBytes() As Byte = File.ReadAllBytes(filePath)
            Dim headerText As String =
                "--" & boundary & vbCrLf &
                "Content-Disposition: form-data; name=""file""; filename=""" & fileName & """" & vbCrLf &
                "Content-Type: application/pdf" & vbCrLf & vbCrLf
            Dim footerText As String = vbCrLf & "--" & boundary & "--" & vbCrLf
            Dim headerBytes() As Byte = Encoding.UTF8.GetBytes(headerText)
            Dim footerBytes() As Byte = Encoding.UTF8.GetBytes(footerText)

            Dim req As HttpWebRequest = CType(WebRequest.Create(uploadUrl), HttpWebRequest)
            req.Method = "POST"
            req.ContentType = "multipart/form-data; boundary=" & boundary
            req.Accept = "application/json"
            req.UserAgent = "AccoBook"
            req.KeepAlive = False
            req.Timeout = 30000
            req.ReadWriteTimeout = 30000
            req.Proxy = Nothing
            req.Headers.Add("X-Upload-Key", UPLOAD_KEY)
            req.ContentLength = headerBytes.Length + fileBytes.Length + footerBytes.Length

            Using reqStream As Stream = req.GetRequestStream()
                reqStream.Write(headerBytes, 0, headerBytes.Length)
                reqStream.Write(fileBytes, 0, fileBytes.Length)
                reqStream.Write(footerBytes, 0, footerBytes.Length)
            End Using

            Dim response As String = ""
            Using httpResp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
                Using respStream As Stream = httpResp.GetResponseStream()
                    Using reader As New StreamReader(respStream, Encoding.UTF8)
                        response = reader.ReadToEnd()
                    End Using
                End Using
            End Using

            If response.Contains("""status"":true") Then

                Try
                    Dim obj As JObject = JObject.Parse(response)
                    If obj("short_url") IsNot Nothing Then
                        Return obj("short_url").ToString()
                    End If
                Catch
                End Try

                Return "SHORT URL NOT FOUND"
            End If

            Return response

        Catch ex As WebException
            Try
                If ex.Response IsNot Nothing Then
                    Using errResp As HttpWebResponse = CType(ex.Response, HttpWebResponse)
                        Using errStream As Stream = errResp.GetResponseStream()
                            Using errReader As New StreamReader(errStream)
                                Return "HTTP " & CInt(errResp.StatusCode).ToString() & " - " & errReader.ReadToEnd()
                            End Using
                        End Using
                    End Using
                End If
            Catch
            End Try
            Return ex.Message
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

End Module
