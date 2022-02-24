Imports MySql.Data.MySqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraBars.Helpers
Imports DevExpress.XtraPrinting
Imports System.Drawing.Printing

Module Module1

    'untuk membuka/menutup koneksi
    Public Sub ExecuteConnection(ByVal OpenStatus As Boolean)
        Dim Connection As New MySqlConnection(My.Settings.ConString)
        If OpenStatus = True Then
            If Connection.State = ConnectionState.Closed Then
                Connection.Open()
            Else
                Connection.Close()
                Connection.Open()
            End If
        Else
            If Connection.State = ConnectionState.Open Then
                Connection.Close()
            End If
        End If
    End Sub


    'mengaktifkan tema untuk form biasa
    Public Sub ExecuteSkin(Optional ByVal SkinName As String = Nothing)
        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        If SkinName = Nothing Then
            UserLookAndFeel.Default.SetSkinStyle(My.Settings.SkinName)
        Else
            UserLookAndFeel.Default.SetSkinStyle(SkinName)
        End If
    End Sub

    'mengaktifkan tema untuk ribbon form
    Public Sub ExecuteSkin(ByVal SkinName As String, ByVal RibbonGalleryBarItem As DevExpress.XtraBars.RibbonGalleryBarItem)
        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        UserLookAndFeel.Default.SetSkinStyle(SkinName)
        SkinHelper.InitSkinGallery(RibbonGalleryBarItem, True)
    End Sub

    'menyimpan tema
    Public Sub ExecuteSkinSave(ByVal SkinName As String)
        If SkinName = "Xmas (Blue)" Then
            SkinName = "Xmas 2008 Blue"
        ElseIf SkinName = "Summer" Then
            SkinName = "Summer 2008"
        ElseIf SkinName = "Office 2013 White" Then
            SkinName = "Office 2013"
        End If
        My.Settings.SkinName = SkinName
        My.Settings.Save()
    End Sub

    'mengaktifkan walpaper
    Public Sub ExecuteWalpaper(ByVal FormName As Object, Optional ByVal ImageLocation As String = Nothing)
        With FormName
            If ImageLocation <> Nothing Then
                .BackgroundImage = Image.FromFile(ImageLocation)
                .BackgroundImageLayout = ImageLayout.Stretch
            End If
        End With
    End Sub

    'menyimpan walpaper
    Public Sub ExecuteWalpaperSave(ByVal RibbonForm As DevExpress.XtraBars.Ribbon.RibbonForm)
        With RibbonForm
            If My.Settings.ImageLocation = Nothing Then
                Dim OFD As New OpenFileDialog
                OFD.Filter = "Image|*.jpg;*.png"
                If OFD.ShowDialog = DialogResult.OK Then
                    .BackgroundImage = Image.FromFile(OFD.FileName)
                    .BackgroundImageLayout = ImageLayout.Stretch
                    My.Settings.ImageLocation = OFD.FileName
                End If
            Else
                .BackgroundImage = Nothing
                My.Settings.ImageLocation = Nothing
            End If
            My.Settings.Save()
        End With
    End Sub

    'mengekseskusi perintah sql
    Public Function ExecuteQuery(ByVal QueryString As String, Optional ByVal DCT As Integer = 30) As DataTable
        Dim Connection As New MySqlConnection(My.Settings.ConString & ";default command timeout=" & DCT)
        Dim DataAdapter As New MySqlDataAdapter(QueryString, Connection)
        Dim DataTable As New DataTable
        DataAdapter.Fill(DataTable)
        Return DataTable
    End Function

    'merapikan tampilan gridcontrol
    Public Sub ExecuteGridViewAllAppearance(ByVal GridView As DevExpress.XtraGrid.Views.Grid.GridView, Optional ByVal GroupFieldNames As String = Nothing, Optional ByVal HiddenFieldNames As String = Nothing, Optional ByVal NumberFormatFieldNames As String = Nothing, Optional ByVal CurrencyFormatFieldNames As String = Nothing, Optional ByVal NumberSumGroupFieldNames As String = Nothing, Optional ByVal CurrencySumGroupFieldNames As String = Nothing, Optional ByVal NumberSumFooterFieldNames As String = Nothing, Optional ByVal CurrencySumFooterFieldNames As String = Nothing, Optional ByVal DenyEditFieldNames As String = Nothing, Optional ByVal SortAscendingFieldNames As String = Nothing, Optional ByVal SortDescendingFieldNames As String = Nothing)
        With GridView
            '.Appearance.FocusedRow.BackColor = Color.YellowGreen
            .OptionsNavigation.UseTabKey = False
            If GroupFieldNames <> Nothing Then
                Dim FieldName = GroupFieldNames.Split(",")
                For Each i In FieldName
                    .Columns(i).Group()
                Next
            End If
            If HiddenFieldNames <> Nothing Then
                Dim FieldName = HiddenFieldNames.Split(",")
                For Each i In FieldName
                    .Columns(i).Visible = False
                Next
            End If
            If NumberFormatFieldNames <> Nothing Then
                Dim FieldName = NumberFormatFieldNames.Split(",")
                For Each i In FieldName
                    .Columns(Microsoft.VisualBasic.Left(i, IIf(Val(InStr(i, "|") <> 0), InStr(i, "|") - 1, Microsoft.VisualBasic.Len(i)))).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                    .Columns(Microsoft.VisualBasic.Left(i, IIf(Val(InStr(i, "|") <> 0), InStr(i, "|") - 1, Microsoft.VisualBasic.Len(i)))).DisplayFormat.FormatString = "n" & Val(Mid(i, InStr(i, "|") + 1, i.Length)) & ""
                Next
            End If
            If CurrencyFormatFieldNames <> Nothing Then
                Dim FieldName = CurrencyFormatFieldNames.Split(",")
                For Each i In FieldName
                    .Columns(Microsoft.VisualBasic.Left(i, IIf(Val(InStr(i, "|") <> 0), InStr(i, "|") - 1, Microsoft.VisualBasic.Len(i)))).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                    .Columns(Microsoft.VisualBasic.Left(i, IIf(Val(InStr(i, "|") <> 0), InStr(i, "|") - 1, Microsoft.VisualBasic.Len(i)))).DisplayFormat.FormatString = "c" & Val(Mid(i, InStr(i, "|") + 1, i.Length)) & ""
                Next
            End If
            If NumberSumGroupFieldNames <> Nothing Then
                Dim FieldName = NumberSumGroupFieldNames.Split(",")
                For Each i In FieldName
                    With .GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, Microsoft.VisualBasic.Left(i, IIf(Val(InStr(i, "|") <> 0), InStr(i, "|") - 1, Microsoft.VisualBasic.Len(i))))
                        .ShowInGroupColumnFooter = GridView.Columns(Microsoft.VisualBasic.Left(i, IIf(Val(InStr(i, "|") <> 0), InStr(i, "|") - 1, Microsoft.VisualBasic.Len(i))))
                        .DisplayFormat = "{0:n" & Val(Mid(i, InStr(i, "|") + 1, i.Length)) & "}"
                    End With
                Next
            End If
            If CurrencySumGroupFieldNames <> Nothing Then
                Dim FieldName = CurrencySumGroupFieldNames.Split(",")
                For Each i In FieldName
                    With .GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, Microsoft.VisualBasic.Left(i, IIf(Val(InStr(i, "|") <> 0), InStr(i, "|") - 1, Microsoft.VisualBasic.Len(i))))
                        .ShowInGroupColumnFooter = GridView.Columns(Microsoft.VisualBasic.Left(i, IIf(Val(InStr(i, "|") <> 0), InStr(i, "|") - 1, Microsoft.VisualBasic.Len(i))))
                        .DisplayFormat = "{0:c" & Val(Mid(i, InStr(i, "|") + 1, i.Length)) & "}"
                    End With
                Next
            End If
            If NumberSumFooterFieldNames <> Nothing Then
                .OptionsView.ShowFooter = True
                Dim FieldName = NumberSumFooterFieldNames.Split(",")
                For Each i In FieldName
                    .Columns(Microsoft.VisualBasic.Left(i, IIf(Val(InStr(i, "|") <> 0), InStr(i, "|") - 1, Microsoft.VisualBasic.Len(i)))).SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "{0:n" & Val(Mid(i, InStr(i, "|") + 1, i.Length)) & "}")
                Next
            End If
            If CurrencySumFooterFieldNames <> Nothing Then
                .OptionsView.ShowFooter = True
                Dim FieldName = CurrencySumFooterFieldNames.Split(",")
                For Each i In FieldName
                    .Columns(Microsoft.VisualBasic.Left(i, IIf(Val(InStr(i, "|") <> 0), InStr(i, "|") - 1, Microsoft.VisualBasic.Len(i)))).SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "{0:c" & Val(Mid(i, InStr(i, "|") + 1, i.Length)) & "}")
                Next
            End If
            If DenyEditFieldNames <> Nothing Then
                Dim FieldName = DenyEditFieldNames.Split(",")
                For Each i In FieldName
                    .Columns(i).OptionsColumn.AllowEdit = False
                Next
            End If
            If SortAscendingFieldNames <> Nothing Then
                Dim FieldName = SortAscendingFieldNames.Split(",")
                For Each i In FieldName
                    .Columns(i).SortOrder = DevExpress.Data.ColumnSortOrder.Ascending
                Next
            End If
            If SortDescendingFieldNames <> Nothing Then
                Dim FieldName = SortDescendingFieldNames.Split(",")
                For Each i In FieldName
                    .Columns(i).SortOrder = DevExpress.Data.ColumnSortOrder.Descending
                Next
            End If
            .ExpandAllGroups()
            .BestFitColumns()
        End With
    End Sub

    'membersihkan inputan yang berupa textedit, comboboxedit
    Public Sub ExecuteInputTextValueClear(ByVal ParamArray TextEditComboBoxEditSpinEditNames() As Object)
        For Each Component In TextEditComboBoxEditSpinEditNames
            Component.Text = Nothing
        Next
    End Sub

    'mengkonversi dari ribbon control ke tree view baik dicentang semua atau tidak dicentang semua
    Public Sub ExecuteTreeViewCheckUncheck(ByVal RibbonControl As DevExpress.XtraBars.Ribbon.RibbonControl, ByVal TreeView As TreeView, ByVal CheckStatus As Boolean)
        With TreeView
            .Nodes.Clear()
            For Each i As DevExpress.XtraBars.Ribbon.RibbonPage In RibbonControl.Pages
                Dim a = .Nodes.Add(i.Text)
                a.Checked = CheckStatus
                For Each ii As DevExpress.XtraBars.Ribbon.RibbonPageGroup In i.Groups
                    Dim b = a.Nodes.Add(ii.Text)
                    b.Checked = CheckStatus
                    For Each iii As DevExpress.XtraBars.BarButtonItemLink In ii.ItemLinks
                        b.Nodes.Add(iii.Caption).Checked = CheckStatus
                    Next
                Next
            Next
            .ExpandAll()
        End With
    End Sub

    'mengkonversi dari NavBarControl l ke tree view baik dicentang semua atau tidak dicentang semua
    Public Sub ExecuteTreeViewCheckUncheck(ByVal NavBarControlName As DevExpress.XtraNavBar.NavBarControl, ByVal TreeView As TreeView, ByVal CheckStatus As Boolean)
        With TreeView
            .CheckBoxes = True
            .Nodes.Clear()
            For Each i As DevExpress.XtraNavBar.NavBarGroup In NavBarControlName.Groups
                Dim a = .Nodes.Add(i.Caption)
                a.Checked = CheckStatus
                For Each ii As DevExpress.XtraNavBar.NavBarItemLink In i.ItemLinks
                    Dim b = a.Nodes.Add(ii.Caption)
                    b.Checked = CheckStatus
                Next
            Next
            .ExpandAll()
        End With
    End Sub

    'mengkonversi dari tree view ke string untuk disimpan ke database
    Public Function ExecuteGetAccess(ByVal TreeView As TreeView) As String
        Dim Access As String = Nothing
        For Each i As TreeNode In TreeView.Nodes
            If i.Checked Then
                Access &= "1:" & i.Text & ";"
            Else
                Access &= "0:" & i.Text & ";"
            End If
            For Each ii As TreeNode In i.Nodes
                If ii.Checked Then
                    Access &= "1:" & ii.Text & ";"
                Else
                    Access &= "0:" & ii.Text & ";"
                End If
                For Each iii As TreeNode In ii.Nodes
                    If iii.Checked Then
                        Access &= "1:" & iii.Text & ";"
                    Else
                        Access &= "0:" & iii.Text & ";"
                    End If
                Next
            Next
        Next
        Return Mid(Access, 1, Access.Length - 1)
    End Function

    'bantuan
    Private Function ExecuteListDetail(ByVal Filter As String, ByVal AllAccess As String, ByVal CharSplit As String) As Double
        Dim Hasil As Boolean
        For Each i In AllAccess.Split(CharSplit)
            If Filter = Mid(i, 3, i.Length - 2) Then
                If Microsoft.VisualBasic.Left(i, 1) = "1" Then
                    Hasil = True
                Else
                    Hasil = False
                End If
                Return Hasil
            End If
        Next
        Return Hasil
    End Function

    'mengkonversi dari string di database ke tree view
    Public Sub ExecuteSetAccessTreeView(ByVal RibbonControl As DevExpress.XtraBars.Ribbon.RibbonControl, ByVal TreeView As TreeView, ByVal StringAccess As String)
        ExecuteTreeViewCheckUncheck(RibbonControl, TreeView, True)
        For Each i As TreeNode In TreeView.Nodes
            If ExecuteListDetail(i.Text, StringAccess, ";") = False Then
                i.Checked = False
            End If
            For Each ii As TreeNode In i.Nodes
                If ExecuteListDetail(ii.Text, StringAccess, ";") = False Then
                    ii.Checked = False
                End If
                For Each iii As TreeNode In ii.Nodes
                    If ExecuteListDetail(iii.Text, StringAccess, ";") = False Then
                        iii.Checked = False
                    End If
                Next
            Next
        Next
    End Sub

    'mengkonversi dari string di database ke tree view untuk navbar
    Public Sub ExecuteSetAccessTreeView(ByVal NavBarControlName As DevExpress.XtraNavBar.NavBarControl, ByVal TreeView As TreeView, ByVal StringAccess As String)
        ExecuteTreeViewCheckUncheck(NavBarControlName, TreeView, True)
        For Each i As TreeNode In TreeView.Nodes
            If ExecuteListDetail(i.Text, StringAccess, ";") = False Then
                i.Checked = False
            End If
            For Each ii As TreeNode In i.Nodes
                If ExecuteListDetail(ii.Text, StringAccess, ";") = False Then
                    ii.Checked = False
                End If
            Next
        Next
    End Sub

    'untuk cek waktu buat form login
    Public Sub ExecuteCheckAccess(ByVal RibbonControl As DevExpress.XtraBars.Ribbon.RibbonControl, ByVal FullFilterAccess As String)
        For Each FilterAccess As String In FullFilterAccess.Split(";")
            For Each i As DevExpress.XtraBars.Ribbon.RibbonPage In RibbonControl.Pages
                If Mid(FilterAccess, 3, FilterAccess.Length - 2) = i.Text Then
                    If Mid(FilterAccess, 3, FilterAccess.Length - 2) = i.Text Then
                        If Microsoft.VisualBasic.Left(FilterAccess, 1) = "0" Then
                            i.Visible = False
                        Else
                            i.Visible = True
                        End If
                    End If
                End If
                For Each ii As DevExpress.XtraBars.Ribbon.RibbonPageGroup In i.Groups
                    If Mid(FilterAccess, 3, FilterAccess.Length - 2) = ii.Text Then
                        If Mid(FilterAccess, 3, FilterAccess.Length - 2) = ii.Text Then
                            If Microsoft.VisualBasic.Left(FilterAccess, 1) = "0" Then
                                ii.Visible = False
                            Else
                                ii.Visible = True
                            End If
                        End If
                    End If
                    For Each iii As DevExpress.XtraBars.BarButtonItemLink In ii.ItemLinks
                        If Mid(FilterAccess, 3, FilterAccess.Length - 2) = iii.Caption Then
                            If Microsoft.VisualBasic.Left(FilterAccess, 1) = "0" Then
                                iii.Visible = False
                            Else
                                iii.Visible = True
                            End If
                        End If
                    Next
                Next
            Next
        Next
    End Sub

    'untuk cek waktu buat form login versi navbar
    Public Sub ExecuteCheckAccess(ByVal NavBarControlName As DevExpress.XtraNavBar.NavBarControl, ByVal FullFilterAccess As String)
        For Each FilterAccess As String In FullFilterAccess.Split(";")
            For Each i As DevExpress.XtraNavBar.NavBarGroup In NavBarControlName.Groups
                If Mid(FilterAccess, 3, FilterAccess.Length - 2) = i.Caption Then
                    If Mid(FilterAccess, 3, FilterAccess.Length - 2) = i.Caption Then
                        If Microsoft.VisualBasic.Left(FilterAccess, 1) = "0" Then
                            i.Visible = False
                        Else
                            i.Visible = True
                        End If
                    End If
                End If
                For Each ii As DevExpress.XtraNavBar.NavBarItemLink In i.ItemLinks
                    If Mid(FilterAccess, 3, FilterAccess.Length - 2) = ii.Caption Then
                        If Mid(FilterAccess, 3, FilterAccess.Length - 2) = ii.Caption Then
                            If Microsoft.VisualBasic.Left(FilterAccess, 1) = "0" Then
                                ii.Visible = False
                            Else
                                ii.Visible = True
                            End If
                        End If
                    End If
                Next
            Next
        Next
    End Sub

    'menyimpan secara permanen data tabel yang sudah dirubah
    Public Sub ExecuteQueryUpdate(ByVal DataTable As DataTable, ByVal TableName As String)
        Dim Connection As New MySqlConnection(My.Settings.ConString)
        Dim DataAdapter As New MySqlDataAdapter("Select * From " & TableName, Connection)
        Dim CommandBuilder As New MySqlCommandBuilder(DataAdapter)
        DataAdapter.Update(DataTable)
    End Sub

    'menyimpan secara permanen data tabel yang sudah dirubah sekaligus merapikan tampilan
    Public Sub ExecuteQueryUpdate(ByVal DataTable As DataTable, ByVal TableName As String, ByVal GridView As DevExpress.XtraGrid.Views.Grid.GridView)
        Dim Connection As New MySqlConnection(My.Settings.ConString)
        Dim DataAdapter As New MySqlDataAdapter("Select * From " & TableName, Connection)
        Dim CommandBuilder As New MySqlCommandBuilder(DataAdapter)
        DataAdapter.Update(DataTable)
        With GridView
            .ExpandAllGroups()
            .BestFitColumns()
        End With
    End Sub

    Public Function ExecuteAutoCode(ByVal TableName As String, ByVal FieldName As String, ByVal InitialCode As String, ByVal DigitFormat As String) As String
        Dim TextCode As String = ""
        Dim DataCode = ExecuteQuery("select max(right(" & FieldName & "," & DigitFormat.Length & ")) as " & FieldName & " from " & TableName & "").Select()
        Try
            TextCode = InitialCode & Format(Right(DataCode(0).Item(FieldName), Len(DigitFormat)) + 1, DigitFormat)

        Catch ex As Exception
            TextCode = InitialCode & Format(1, DigitFormat)
        End Try
        Return TextCode
    End Function

    Public Sub ExecuteComboBoxList(ByVal TableName As String, ByVal FieldName As String, ByVal ComboBoxEditName As DevExpress.XtraEditors.ComboBoxEdit, Optional ByVal Criteria As String = Nothing, Optional ByVal ListAdd As String = Nothing)
        ComboBoxEditName.Properties.Items.Clear()
        If ListAdd <> Nothing Then
            Dim List = ListAdd.Split(",")
            For Each i In List
                ComboBoxEditName.Properties.Items.Add(i)
            Next
        End If
        Dim QueryString As String
        If Criteria <> Nothing Then
            QueryString = "select distinct(" & FieldName & ") as " & FieldName & " from " & TableName & " where " & Criteria
        Else
            QueryString = "select distinct(" & FieldName & ") as " & FieldName & " from " & TableName
        End If
        Dim Connection As New MySqlConnection(My.Settings.ConString)
        Dim DataAdapter As New MySqlDataAdapter(QueryString, Connection)
        Dim DataTable As New DataTable
        DataAdapter.Fill(DataTable)
        For Each isi In (From rs In DataTable Select rs(FieldName)).Distinct
            ComboBoxEditName.Properties.Items.Add(isi)
        Next
    End Sub

    Public Sub ExecuteGridControlPreview(ByVal GridControl As IPrintable, ByVal GridControlLookAndFeel As UserLookAndFeel, Optional ByVal StatusLandScape As Boolean = False, Optional ByVal PaperKind As System.Drawing.Printing.PaperKind = PaperKind.A4, Optional ByVal LeftMargin As Double = 50, Optional ByVal RightMargin As Double = 50, Optional ByVal TopMargin As Double = 50, Optional ByVal ButtomMargin As Double = 50)
        Dim Link As New PrintableComponentLink() With {.PrintingSystemBase = New PrintingSystem(), .Component = GridControl, .Landscape = StatusLandScape, .PaperKind = PaperKind, .Margins = New Margins(LeftMargin, RightMargin, TopMargin, ButtomMargin)}
        Link.ShowRibbonPreview(GridControlLookAndFeel)
    End Sub
End Module
