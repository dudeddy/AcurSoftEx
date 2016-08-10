Namespace AcurSoft.XtraGrid.Views.Grid.Extenders

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ColumnSummaryConfig
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
            Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
            Me.btn_help_display_format = New DevExpress.XtraEditors.SimpleButton()
            Me.btn_help_expression = New DevExpress.XtraEditors.SimpleButton()
            Me.edExpression = New DevExpress.XtraEditors.MemoEdit()
            Me.edFieldName = New DevExpress.XtraEditors.LookUpEdit()
            Me.edTop = New DevExpress.XtraEditors.SpinEdit()
            Me.edType = New DevExpress.XtraEditors.LookUpEdit()
            Me.edDisplayFormat = New DevExpress.XtraEditors.TextEdit()
            Me.btnCancel = New DevExpress.XtraEditors.SimpleButton()
            Me.btnOk = New DevExpress.XtraEditors.SimpleButton()
            Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.EmptySpaceItem = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.lciTop = New DevExpress.XtraLayout.LayoutControlItem()
            Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.lcgExpression = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.EmptySpaceItem2 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.lciExpression = New DevExpress.XtraLayout.LayoutControlItem()
            Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.lcgDisplayFormat = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.SparklineInfosEditor1 = New AcurSoft.XtraGrid.SparklineInfosEditor()
            Me.lciSparkline = New DevExpress.XtraLayout.LayoutControlItem()
            CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.LayoutControl1.SuspendLayout()
            CType(Me.edExpression.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.edFieldName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.edTop.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.edType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.edDisplayFormat.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.EmptySpaceItem, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.lciTop, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.lcgExpression, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.lciExpression, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.lcgDisplayFormat, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.lciSparkline, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'LayoutControl1
            '
            Me.LayoutControl1.AllowCustomization = False
            Me.LayoutControl1.Controls.Add(Me.SparklineInfosEditor1)
            Me.LayoutControl1.Controls.Add(Me.btn_help_display_format)
            Me.LayoutControl1.Controls.Add(Me.btn_help_expression)
            Me.LayoutControl1.Controls.Add(Me.edExpression)
            Me.LayoutControl1.Controls.Add(Me.edFieldName)
            Me.LayoutControl1.Controls.Add(Me.edTop)
            Me.LayoutControl1.Controls.Add(Me.edType)
            Me.LayoutControl1.Controls.Add(Me.edDisplayFormat)
            Me.LayoutControl1.Controls.Add(Me.btnCancel)
            Me.LayoutControl1.Controls.Add(Me.btnOk)
            Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
            Me.LayoutControl1.Name = "LayoutControl1"
            Me.LayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(424, 89, 424, 466)
            Me.LayoutControl1.Root = Me.LayoutControlGroup1
            Me.LayoutControl1.Size = New System.Drawing.Size(347, 415)
            Me.LayoutControl1.TabIndex = 0
            Me.LayoutControl1.Text = "LayoutControl1"
            '
            'btn_help_display_format
            '
            Me.btn_help_display_format.Location = New System.Drawing.Point(290, 52)
            Me.btn_help_display_format.Name = "btn_help_display_format"
            Me.btn_help_display_format.Size = New System.Drawing.Size(36, 20)
            Me.btn_help_display_format.StyleController = Me.LayoutControl1
            Me.btn_help_display_format.TabIndex = 12
            Me.btn_help_display_format.Text = "?"
            '
            'btn_help_expression
            '
            Me.btn_help_expression.Location = New System.Drawing.Point(290, 320)
            Me.btn_help_expression.Name = "btn_help_expression"
            Me.btn_help_expression.Size = New System.Drawing.Size(36, 20)
            Me.btn_help_expression.StyleController = Me.LayoutControl1
            Me.btn_help_expression.TabIndex = 11
            Me.btn_help_expression.Text = "?"
            '
            'edExpression
            '
            Me.edExpression.Location = New System.Drawing.Point(4, 344)
            Me.edExpression.Name = "edExpression"
            Me.edExpression.Size = New System.Drawing.Size(322, 46)
            Me.edExpression.StyleController = Me.LayoutControl1
            Me.edExpression.TabIndex = 10
            '
            'edFieldName
            '
            Me.edFieldName.Location = New System.Drawing.Point(85, 4)
            Me.edFieldName.Name = "edFieldName"
            Me.edFieldName.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.edFieldName.Properties.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("caption", "caption")})
            Me.edFieldName.Properties.NullText = ""
            Me.edFieldName.Size = New System.Drawing.Size(241, 20)
            Me.edFieldName.StyleController = Me.LayoutControl1
            Me.edFieldName.TabIndex = 9
            '
            'edTop
            '
            Me.edTop.EditValue = New Decimal(New Integer() {5, 0, 0, 0})
            Me.edTop.Location = New System.Drawing.Point(85, 76)
            Me.edTop.Name = "edTop"
            Me.edTop.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.edTop.Properties.IsFloatValue = False
            Me.edTop.Properties.Mask.EditMask = "N00"
            Me.edTop.Properties.MaxValue = New Decimal(New Integer() {1000000, 0, 0, 0})
            Me.edTop.Properties.MinValue = New Decimal(New Integer() {1, 0, 0, 0})
            Me.edTop.Size = New System.Drawing.Size(241, 20)
            Me.edTop.StyleController = Me.LayoutControl1
            Me.edTop.TabIndex = 8
            '
            'edType
            '
            Me.edType.Location = New System.Drawing.Point(85, 28)
            Me.edType.Name = "edType"
            Me.edType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.edType.Properties.NullText = ""
            Me.edType.Properties.ShowFooter = False
            Me.edType.Properties.ShowHeader = False
            Me.edType.Size = New System.Drawing.Size(241, 20)
            Me.edType.StyleController = Me.LayoutControl1
            Me.edType.TabIndex = 7
            '
            'edDisplayFormat
            '
            Me.edDisplayFormat.Location = New System.Drawing.Point(85, 52)
            Me.edDisplayFormat.Name = "edDisplayFormat"
            Me.edDisplayFormat.Size = New System.Drawing.Size(201, 20)
            Me.edDisplayFormat.StyleController = Me.LayoutControl1
            Me.edDisplayFormat.TabIndex = 6
            '
            'btnCancel
            '
            Me.btnCancel.Location = New System.Drawing.Point(255, 404)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(71, 22)
            Me.btnCancel.StyleController = Me.LayoutControl1
            Me.btnCancel.TabIndex = 5
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Location = New System.Drawing.Point(180, 404)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(71, 22)
            Me.btnOk.StyleController = Me.LayoutControl1
            Me.btnOk.TabIndex = 4
            Me.btnOk.Text = "Ok"
            '
            'LayoutControlGroup1
            '
            Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
            Me.LayoutControlGroup1.GroupBordersVisible = False
            Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem4, Me.EmptySpaceItem1, Me.LayoutControlItem1, Me.LayoutControlItem2, Me.EmptySpaceItem, Me.lciTop, Me.LayoutControlItem5, Me.lcgExpression, Me.lciSparkline, Me.lcgDisplayFormat})
            Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
            Me.LayoutControlGroup1.Name = "Root"
            Me.LayoutControlGroup1.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
            Me.LayoutControlGroup1.Size = New System.Drawing.Size(330, 430)
            Me.LayoutControlGroup1.TextVisible = False
            '
            'LayoutControlItem3
            '
            Me.LayoutControlItem3.Control = Me.edDisplayFormat
            Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 0)
            Me.LayoutControlItem3.Name = "LayoutControlItem3"
            Me.LayoutControlItem3.Size = New System.Drawing.Size(286, 24)
            Me.LayoutControlItem3.Text = "Display Format :"
            Me.LayoutControlItem3.TextSize = New System.Drawing.Size(78, 13)
            '
            'LayoutControlItem4
            '
            Me.LayoutControlItem4.Control = Me.edType
            Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 24)
            Me.LayoutControlItem4.Name = "LayoutControlItem4"
            Me.LayoutControlItem4.Size = New System.Drawing.Size(326, 24)
            Me.LayoutControlItem4.Text = "Type :"
            Me.LayoutControlItem4.TextSize = New System.Drawing.Size(78, 13)
            '
            'EmptySpaceItem1
            '
            Me.EmptySpaceItem1.AllowHotTrack = False
            Me.EmptySpaceItem1.Location = New System.Drawing.Point(0, 400)
            Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
            Me.EmptySpaceItem1.Size = New System.Drawing.Size(176, 26)
            Me.EmptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
            '
            'LayoutControlItem1
            '
            Me.LayoutControlItem1.Control = Me.btnOk
            Me.LayoutControlItem1.Location = New System.Drawing.Point(176, 400)
            Me.LayoutControlItem1.MaxSize = New System.Drawing.Size(75, 26)
            Me.LayoutControlItem1.MinSize = New System.Drawing.Size(75, 26)
            Me.LayoutControlItem1.Name = "LayoutControlItem1"
            Me.LayoutControlItem1.Size = New System.Drawing.Size(75, 26)
            Me.LayoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItem1.TextVisible = False
            '
            'LayoutControlItem2
            '
            Me.LayoutControlItem2.Control = Me.btnCancel
            Me.LayoutControlItem2.Location = New System.Drawing.Point(251, 400)
            Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(75, 26)
            Me.LayoutControlItem2.MinSize = New System.Drawing.Size(75, 26)
            Me.LayoutControlItem2.Name = "LayoutControlItem2"
            Me.LayoutControlItem2.Size = New System.Drawing.Size(75, 26)
            Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItem2.TextVisible = False
            '
            'EmptySpaceItem
            '
            Me.EmptySpaceItem.AllowHotTrack = False
            Me.EmptySpaceItem.AllowHtmlStringInCaption = True
            Me.EmptySpaceItem.AppearanceItemCaption.Options.UseTextOptions = True
            Me.EmptySpaceItem.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
            Me.EmptySpaceItem.Location = New System.Drawing.Point(0, 390)
            Me.EmptySpaceItem.Name = "EmptySpaceItem"
            Me.EmptySpaceItem.Size = New System.Drawing.Size(326, 10)
            Me.EmptySpaceItem.StartNewLine = True
            Me.EmptySpaceItem.TextSize = New System.Drawing.Size(0, 0)
            '
            'lciTop
            '
            Me.lciTop.Control = Me.edTop
            Me.lciTop.Location = New System.Drawing.Point(0, 72)
            Me.lciTop.Name = "lciTop"
            Me.lciTop.Size = New System.Drawing.Size(326, 24)
            Me.lciTop.TextSize = New System.Drawing.Size(78, 13)
            Me.lciTop.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            '
            'LayoutControlItem5
            '
            Me.LayoutControlItem5.Control = Me.edFieldName
            Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 0)
            Me.LayoutControlItem5.Name = "LayoutControlItem5"
            Me.LayoutControlItem5.Size = New System.Drawing.Size(326, 24)
            Me.LayoutControlItem5.Text = "Field Name :"
            Me.LayoutControlItem5.TextSize = New System.Drawing.Size(78, 13)
            '
            'lcgExpression
            '
            Me.lcgExpression.GroupBordersVisible = False
            Me.lcgExpression.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.EmptySpaceItem2, Me.lciExpression, Me.LayoutControlItem6})
            Me.lcgExpression.Location = New System.Drawing.Point(0, 316)
            Me.lcgExpression.Name = "lcgExpression"
            Me.lcgExpression.Size = New System.Drawing.Size(326, 74)
            Me.lcgExpression.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            '
            'EmptySpaceItem2
            '
            Me.EmptySpaceItem2.AllowHotTrack = False
            Me.EmptySpaceItem2.Location = New System.Drawing.Point(0, 0)
            Me.EmptySpaceItem2.Name = "EmptySpaceItem2"
            Me.EmptySpaceItem2.Size = New System.Drawing.Size(286, 24)
            Me.EmptySpaceItem2.Text = "Expression :"
            Me.EmptySpaceItem2.TextSize = New System.Drawing.Size(78, 0)
            Me.EmptySpaceItem2.TextVisible = True
            '
            'lciExpression
            '
            Me.lciExpression.Control = Me.edExpression
            Me.lciExpression.Location = New System.Drawing.Point(0, 24)
            Me.lciExpression.MinSize = New System.Drawing.Size(14, 50)
            Me.lciExpression.Name = "lciExpression"
            Me.lciExpression.Size = New System.Drawing.Size(326, 50)
            Me.lciExpression.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.lciExpression.Text = "Expression :"
            Me.lciExpression.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.lciExpression.TextLocation = DevExpress.Utils.Locations.Top
            Me.lciExpression.TextSize = New System.Drawing.Size(0, 0)
            Me.lciExpression.TextToControlDistance = 0
            Me.lciExpression.TextVisible = False
            '
            'LayoutControlItem6
            '
            Me.LayoutControlItem6.Control = Me.btn_help_expression
            Me.LayoutControlItem6.Location = New System.Drawing.Point(286, 0)
            Me.LayoutControlItem6.MaxSize = New System.Drawing.Size(40, 24)
            Me.LayoutControlItem6.MinSize = New System.Drawing.Size(40, 24)
            Me.LayoutControlItem6.Name = "LayoutControlItem6"
            Me.LayoutControlItem6.Size = New System.Drawing.Size(40, 24)
            Me.LayoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.LayoutControlItem6.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItem6.TextVisible = False
            '
            'LayoutControlItem7
            '
            Me.LayoutControlItem7.Control = Me.btn_help_display_format
            Me.LayoutControlItem7.Location = New System.Drawing.Point(286, 0)
            Me.LayoutControlItem7.MaxSize = New System.Drawing.Size(40, 24)
            Me.LayoutControlItem7.MinSize = New System.Drawing.Size(40, 24)
            Me.LayoutControlItem7.Name = "LayoutControlItem7"
            Me.LayoutControlItem7.Size = New System.Drawing.Size(40, 24)
            Me.LayoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.LayoutControlItem7.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItem7.TextVisible = False
            '
            'lcgDisplayFormat
            '
            Me.lcgDisplayFormat.GroupBordersVisible = False
            Me.lcgDisplayFormat.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem7, Me.LayoutControlItem3})
            Me.lcgDisplayFormat.Location = New System.Drawing.Point(0, 48)
            Me.lcgDisplayFormat.Name = "lcgDisplayFormat"
            Me.lcgDisplayFormat.Size = New System.Drawing.Size(326, 24)
            '
            'SparklineInfosEditor1
            '
            Me.SparklineInfosEditor1.Infos = Nothing
            Me.SparklineInfosEditor1.Location = New System.Drawing.Point(4, 100)
            Me.SparklineInfosEditor1.Name = "SparklineInfosEditor1"
            Me.SparklineInfosEditor1.Size = New System.Drawing.Size(322, 216)
            Me.SparklineInfosEditor1.TabIndex = 13
            '
            'lciSparkline
            '
            Me.lciSparkline.Control = Me.SparklineInfosEditor1
            Me.lciSparkline.Location = New System.Drawing.Point(0, 96)
            Me.lciSparkline.Name = "lciSparkline"
            Me.lciSparkline.Size = New System.Drawing.Size(326, 220)
            Me.lciSparkline.TextSize = New System.Drawing.Size(0, 0)
            Me.lciSparkline.TextVisible = False
            Me.lciSparkline.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            '
            'ColumnSummaryConfig
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(347, 415)
            Me.Controls.Add(Me.LayoutControl1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
            Me.Name = "ColumnSummaryConfig"
            Me.ShowIcon = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Column Summary Config"
            CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.LayoutControl1.ResumeLayout(False)
            CType(Me.edExpression.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.edFieldName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.edTop.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.edType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.edDisplayFormat.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.EmptySpaceItem, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.lciTop, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.lcgExpression, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.lciExpression, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.lcgDisplayFormat, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.lciSparkline, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
        Friend WithEvents edType As DevExpress.XtraEditors.LookUpEdit
        Friend WithEvents edDisplayFormat As DevExpress.XtraEditors.TextEdit
        Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents btnOk As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
        Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
        Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents EmptySpaceItem As DevExpress.XtraLayout.EmptySpaceItem
        Friend WithEvents edTop As DevExpress.XtraEditors.SpinEdit
        Friend WithEvents lciTop As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents edFieldName As DevExpress.XtraEditors.LookUpEdit
        Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents edExpression As DevExpress.XtraEditors.MemoEdit
        Friend WithEvents lciExpression As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents btn_help_expression As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents lcgExpression As DevExpress.XtraLayout.LayoutControlGroup
        Friend WithEvents EmptySpaceItem2 As DevExpress.XtraLayout.EmptySpaceItem
        Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents btn_help_display_format As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents SparklineInfosEditor1 As SparklineInfosEditor
        Friend WithEvents lciSparkline As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents lcgDisplayFormat As DevExpress.XtraLayout.LayoutControlGroup
    End Class
End Namespace
