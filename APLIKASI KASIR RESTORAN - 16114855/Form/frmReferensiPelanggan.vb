Public Class frmReferensiPelanggan
    Dim Tampung As New DataTable
    Dim StatusDataBaru As Boolean

    Sub TampilTabel()
        Tampung = ExecuteQuery("select ID_Pelanggan,Nama_Pelanggan,Alamat,Nomor_Telepon,Deskripsi from pelanggan")
        GridControl1.DataSource = Tampung
        ExecuteGridViewAllAppearance(GridView1)
    End Sub

    Sub DataBaru()
        StatusDataBaru = True
        ExecuteInputTextValueClear(txtNamaPelanggan, txtAlamat, txtNoTelp, txtDeskripsi)
        txtID.Text = ExecuteAutoCode("pelanggan", "ID_Pelanggan", "PLG", "000000")
        TampilTabel()
        txtNamaPelanggan.Focus()
    End Sub

    Sub Simpan()
        If txtID.Text = "" Or txtNamaPelanggan.Text = "" Or txtAlamat.Text = "" Then
            MessageBox.Show("ID Pelanggan, Nama Pelanggan dan Alamat Harus Diisi!!", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If StatusDataBaru = True Then
            Dim Cari = Tampung.Select("ID_Pelanggan='" & txtID.Text & "'")
            If Cari.Length <= 0 Then
                'INSERT
                ExecuteQuery("INSERT INTO pelanggan(ID_Pelanggan,Nama_Pelanggan,Alamat,Nomor_Telepon,Deskripsi) VALUES ('" & txtID.Text & "','" & txtNamaPelanggan.Text & "','" & txtAlamat.Text & "','" & txtNoTelp.Text & "','" & txtDeskripsi.Text & "')")
                DataBaru()
                MessageBox.Show("DataBerhasil Disimpan", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Data Gagal Disimpan." & vbCrLf & "Data sudah ada dalam database.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            'UPDATE
            ExecuteQuery("UPDATE pelanggan SET Nama_Pelanggan='" & txtNamaPelanggan.Text & "',Alamat='" & txtAlamat.Text & "',Nomor_Telepon='" & txtNoTelp.Text & "',Deskripsi='" & txtDeskripsi.Text & "' WHERE ID_Pelanggan='" & txtID.Text & "' ")
            DataBaru()
            MessageBox.Show("DataBerhasil Disimpan", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If
    End Sub

    Sub Ubah()
        If GridView1.RowCount > 0 Then
            StatusDataBaru = False
            txtID.Text = GridView1.GetFocusedRowCellValue("ID_Pelanggan")
            txtNamaPelanggan.Text = GridView1.GetFocusedRowCellValue("Nama_Pelanggan")
            txtAlamat.Text = GridView1.GetFocusedRowCellValue("Alamat")
            txtNoTelp.Text = GridView1.GetFocusedRowCellValue("Nomor_Telepon")
            txtDeskripsi.Text = GridView1.GetFocusedRowCellValue("Deskripsi")
            txtNamaPelanggan.Focus()
        Else
            MessageBox.Show("Tidak ada data yang dipilih", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Sub Hapus()
        If GridView1.RowCount > 0 Then
            If MessageBox.Show("Data akan dihapus. Lanjutkan?", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                'DELETE
                ExecuteQuery("DELETE FROM pelanggan WHERE ID_Pelanggan='" & GridView1.GetFocusedRowCellValue("ID_Pelanggan") & "'")
                DataBaru()
            End If
        Else
            MessageBox.Show("Tidak ada data yang dipilih!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Sub Cetak()
        GridControl1.ShowRibbonPrintPreview()
    End Sub

    Private Sub frmReferensiPelanggan_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case e.Control And Keys.Enter
                Simpan()
            Case Keys.F5
                DataBaru()
            Case Keys.Escape
                Me.Close()
            Case Keys.F2
                Ubah()
            Case e.Control And Keys.Delete
                Hapus()
            Case Keys.F8
                Cetak()
        End Select
    End Sub

    Private Sub frmReferensiPelanggan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        txtID.ReadOnly = True
        txtID.TabStop = False
        DataBaru()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Simpan()

    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        Me.Close()

    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        DataBaru()

    End Sub

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Ubah()

    End Sub

    Private Sub BarButtonItem3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Hapus()

    End Sub

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Cetak()

    End Sub
End Class