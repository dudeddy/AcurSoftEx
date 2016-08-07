Imports AcurSoft.XtraGrid
Imports AcurSoft.XtraGrid.Views.Grid

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class XtraForm1
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim RangeControlRange1 As DevExpress.XtraEditors.RangeControlRange = New DevExpress.XtraEditors.RangeControlRange()
        Dim LineSparklineView1 As DevExpress.Sparkline.LineSparklineView = New DevExpress.Sparkline.LineSparklineView()
        Me.RangeControl1 = New DevExpress.XtraEditors.RangeControl()
        Me.MonthEdit1 = New DevExpress.XtraScheduler.UI.MonthEdit()
        Me.GridControlEx1 = New AcurSoft.XtraGrid.GridControlEx()
        Me.GridViewEx1 = New AcurSoft.XtraGrid.Views.Grid.GridViewEx()
        Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
        Me.XtraTabPage1 = New DevExpress.XtraTab.XtraTabPage()
        Me.SimpleButton6 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton5 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton4 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton3 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.XtraTabPage2 = New DevExpress.XtraTab.XtraTabPage()
        Me.SparklineEdit1 = New DevExpress.XtraEditors.SparklineEdit()
        CType(Me.RangeControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MonthEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControlEx1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewEx1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabControl1.SuspendLayout()
        Me.XtraTabPage1.SuspendLayout()
        CType(Me.SparklineEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RangeControl1
        '
        Me.RangeControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RangeControl1.Location = New System.Drawing.Point(0, 0)
        Me.RangeControl1.Name = "RangeControl1"
        RangeControlRange1.Maximum = New Date(2016, 6, 5, 0, 0, 0, 0)
        RangeControlRange1.Minimum = New Date(2016, 5, 27, 0, 0, 0, 0)
        RangeControlRange1.Owner = Me.RangeControl1
        Me.RangeControl1.SelectedRange = RangeControlRange1
        Me.RangeControl1.Size = New System.Drawing.Size(889, 91)
        Me.RangeControl1.TabIndex = 0
        Me.RangeControl1.Text = "RangeControl1"
        '
        'MonthEdit1
        '
        Me.MonthEdit1.Location = New System.Drawing.Point(13, 13)
        Me.MonthEdit1.Name = "MonthEdit1"
        Me.MonthEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.MonthEdit1.Size = New System.Drawing.Size(103, 20)
        Me.MonthEdit1.TabIndex = 0
        '
        'GridControlEx1
        '
        Me.GridControlEx1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControlEx1.Location = New System.Drawing.Point(0, 0)
        Me.GridControlEx1.MainView = Me.GridViewEx1
        Me.GridControlEx1.Name = "GridControlEx1"
        Me.GridControlEx1.Size = New System.Drawing.Size(895, 353)
        Me.GridControlEx1.TabIndex = 1
        Me.GridControlEx1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewEx1})
        '
        'GridViewEx1
        '
        Me.GridViewEx1.FindControl = Nothing
        Me.GridViewEx1.GridControl = Me.GridControlEx1
        Me.GridViewEx1.Name = "GridViewEx1"
        '
        'XtraTabControl1
        '
        Me.XtraTabControl1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.XtraTabControl1.Location = New System.Drawing.Point(0, 353)
        Me.XtraTabControl1.Name = "XtraTabControl1"
        Me.XtraTabControl1.SelectedTabPage = Me.XtraTabPage1
        Me.XtraTabControl1.Size = New System.Drawing.Size(895, 119)
        Me.XtraTabControl1.TabIndex = 8
        Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.XtraTabPage1, Me.XtraTabPage2})
        '
        'XtraTabPage1
        '
        Me.XtraTabPage1.Controls.Add(Me.SparklineEdit1)
        Me.XtraTabPage1.Controls.Add(Me.SimpleButton6)
        Me.XtraTabPage1.Controls.Add(Me.SimpleButton5)
        Me.XtraTabPage1.Controls.Add(Me.SimpleButton4)
        Me.XtraTabPage1.Controls.Add(Me.SimpleButton3)
        Me.XtraTabPage1.Controls.Add(Me.SimpleButton2)
        Me.XtraTabPage1.Controls.Add(Me.SimpleButton1)
        Me.XtraTabPage1.Controls.Add(Me.RangeControl1)
        Me.XtraTabPage1.Name = "XtraTabPage1"
        Me.XtraTabPage1.Size = New System.Drawing.Size(889, 91)
        Me.XtraTabPage1.Text = "XtraTabPage1"
        '
        'SimpleButton6
        '
        Me.SimpleButton6.Location = New System.Drawing.Point(700, 48)
        Me.SimpleButton6.Name = "SimpleButton6"
        Me.SimpleButton6.Size = New System.Drawing.Size(75, 23)
        Me.SimpleButton6.TabIndex = 6
        Me.SimpleButton6.Text = "SimpleButton6"
        '
        'SimpleButton5
        '
        Me.SimpleButton5.Location = New System.Drawing.Point(700, 19)
        Me.SimpleButton5.Name = "SimpleButton5"
        Me.SimpleButton5.Size = New System.Drawing.Size(75, 23)
        Me.SimpleButton5.TabIndex = 5
        Me.SimpleButton5.Text = "SimpleButton5"
        '
        'SimpleButton4
        '
        Me.SimpleButton4.Location = New System.Drawing.Point(502, 49)
        Me.SimpleButton4.Name = "SimpleButton4"
        Me.SimpleButton4.Size = New System.Drawing.Size(75, 23)
        Me.SimpleButton4.TabIndex = 4
        Me.SimpleButton4.Text = "restore layout"
        '
        'SimpleButton3
        '
        Me.SimpleButton3.Location = New System.Drawing.Point(502, 19)
        Me.SimpleButton3.Name = "SimpleButton3"
        Me.SimpleButton3.Size = New System.Drawing.Size(75, 23)
        Me.SimpleButton3.TabIndex = 3
        Me.SimpleButton3.Text = "save layout"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Location = New System.Drawing.Point(3, 19)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(141, 23)
        Me.SimpleButton2.TabIndex = 2
        Me.SimpleButton2.Text = "SimpleButton2"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Location = New System.Drawing.Point(3, 73)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(163, 23)
        Me.SimpleButton1.TabIndex = 1
        Me.SimpleButton1.Text = "SimpleButton1"
        '
        'XtraTabPage2
        '
        Me.XtraTabPage2.Name = "XtraTabPage2"
        Me.XtraTabPage2.Size = New System.Drawing.Size(889, 91)
        Me.XtraTabPage2.Text = "XtraTabPage2"
        '
        'SparklineEdit1
        '
        Me.SparklineEdit1.Location = New System.Drawing.Point(201, 19)
        Me.SparklineEdit1.Name = "SparklineEdit1"
        Me.SparklineEdit1.Properties.View = LineSparklineView1
        Me.SparklineEdit1.Size = New System.Drawing.Size(191, 33)
        Me.SparklineEdit1.TabIndex = 9
        '
        'XtraForm1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(895, 472)
        Me.Controls.Add(Me.GridControlEx1)
        Me.Controls.Add(Me.MonthEdit1)
        Me.Controls.Add(Me.XtraTabControl1)
        Me.Name = "XtraForm1"
        Me.Text = "XtraForm1"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.RangeControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MonthEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControlEx1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewEx1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabControl1.ResumeLayout(False)
        Me.XtraTabPage1.ResumeLayout(False)
        CType(Me.SparklineEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents MonthEdit1 As DevExpress.XtraScheduler.UI.MonthEdit
    Friend WithEvents GridControlEx1 As GridControlEx
    Friend WithEvents GridViewEx1 As GridViewEx
    Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents XtraTabPage1 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents RangeControl1 As DevExpress.XtraEditors.RangeControl
    Friend WithEvents XtraTabPage2 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton4 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton5 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton6 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SparklineEdit1 As DevExpress.XtraEditors.SparklineEdit
End Class
