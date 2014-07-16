Imports System.Windows.Forms
Imports System.Collections.CollectionBase 'used in buttonarray
Imports System.Resources 'image load
Imports System.Timers

Imports System.Configuration


Public Class Test

    Inherits System.Windows.Forms.Form
    
    Public Shared Xsize as integer = 15 ' 0 1 2 .... Xsize
    Public Shared  Ysize as integer = 15 ' 0 1 2 .... Ysize
    Public Shared  minetotals as integer = 16
    
    
    dim fsize as integer = (Xsize+1)*(Ysize+1) 'field size
    
    dim cleanBurrons as Boolean = True   
    dim timestopped as boolean = true
    dim mousepressed as boolean = false
    dim myevent as MouseEventArgs
       
    
       
    dim smile as System.Drawing.Image    =  system.drawing.image.fromfile("smile.png")
    dim cat as System.Drawing.Image    =  system.drawing.image.fromfile("cat.png")    
    dim dog as System.Drawing.Image    =  system.drawing.image.fromfile("dog.png")    
    
    private b as New Button()
    private c as New Button()
    private d as New Button()
    
    Dim Label1 as New Label()
    Dim Label2 as New Label()
    
    Private Shared aTimer As System.Timers.Timer
    Private _elapseStartTime As DateTime
    Dim elapsedtime as System.TimeSpan
    
    'Dim MyControlArray as ButtonArray
    
    public mybuttarray(Xsize,Ysize) as button
     
    
    Public Sub New()
    
        MyBase.New()
        MyBase.Topmost = True
        MyBase.Text = "Это Заголовок формы: Игра в сапера на минном поле"
		MyBase.AutoScroll = True
		
		'MyBase.Size = new System.Drawing.Size(520,520)
        MyBase.Size = new System.Drawing.Size(20 + (Xsize+2)*16 +20, 130 + (Ysize+2)*16 + 20 + 20)
        
       ' MyControlArray = New ButtonArray(Me)
       ' MyControlArray.AddNewButton()
       ' MyControlArray(0).BackColor = System.Drawing.Color.Red
        
        b.Dock = DockStyle.Top
        b.Text = "Это Кнопка b: Создать пустое поле/Удалить поле"
        addhandler b.Click, addressof b_click    

        c.Dock = DockStyle.Top
        c.Text = "Это Кнопка c: Добавить немного мин"
        addhandler c.Click, addressof c_click         

        d.Dock = DockStyle.Top
        d.Text = "Это Кнопка d: Остановить/Запустить таймер"
        addhandler d.Click, addressof d_click      
                

        Label1.BackColor = System.Drawing.Color.LightGray
        Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        'Label1.Width = 60
        Label1.Text = "Это метка Label1: Таймер"
        'Label1.location = new System.Drawing.Point(20, 77)
        Label1.Dock = DockStyle.Top
    
    
        Label2.BackColor = System.Drawing.Color.DarkGray
        Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Label2.Text = "Это метка Label2: Статус игры"
        Label2.Dock = DockStyle.Top    
    
    
        MyBase.Controls.Add(Label2)
        MyBase.Controls.Add(Label1)
 
 
        MyBase.Controls.Add(d)
        MyBase.Controls.Add(c)
        MyBase.Controls.Add(b)

 
        call settimer
        call gobuttons
        call setmines
        
        MyBase.ShowDialog()                                
            
        'b.PerformClick()  
        
            
    End sub


    public sub settimer
        ' Create a timer with a ten second interval.
        aTimer = New System.Timers.Timer(10000)

        ' Hook up the Elapsed event for the timer. 
        AddHandler aTimer.Elapsed, AddressOf OnTimedEvent

        ' Set the Interval to 2 seconds (2000 milliseconds).
        aTimer.Interval = 1000
        
        aTimer.SynchronizingObject = me
        
        _elapseStartTime = DateTime.Now
  
    end sub

    

    
    public sub gobuttons
    dim i,j as integer
        
    for i = 0 to Xsize
    
        for j = 0 to Ysize
 
            if cleanBurrons then ' when new buttons needed

                console.write (". Nothing, i=" & CStr(i) & ";j=" & CStr(j))
                mybuttarray(i,j) = new button()
                mybuttarray(i,j).location = new System.Drawing.Point(20 + i*16, 130 + j*16)
                mybuttarray(i,j).Name = ""                
                mybuttarray(i,j).Tag = New Integer() {i, j, 0, 0, 0} 'mine, clicked, flagged
                mybuttarray(i,j).Size = new System.Drawing.Size(16,16)
                mybuttarray(i,j).Capture = false
                mybuttarray(i,j).FlatStyle = FlatStyle.Standard
                MyBase.Controls.Add(mybuttarray(i,j))
                addhandler mybuttarray(i,j).MouseDown, addressof butt_clicker 
                
          
            else  ' clear buttons here
                
                console.write ("Dispose from button {0}, {1} ; " , i ,j)
                MyBase.Controls.Remove(mybuttarray(i,j))
                mybuttarray(i,j).Tag = New Integer() {i, j, -1, -1 ,-1}
                mybuttarray(i,j).Dispose()
                console.write ("what's left is {0} ; " , mybuttarray(i,j).Tag(2))
                
            end if
              
            
                             
        next j
    
    next i
    
    cleanBurrons = not cleanBurrons 'flip state
    
    end sub

   
    public function randomValue(byref lowerbound as integer,byref upperbound as integer) as integer
        randomValue = CInt(Math.Floor((upperbound - lowerbound + 1) * Rnd())) + lowerbound
    end function    


    
    public sub setmines
    
        dim i,j,mineset as integer
        dim thisplace as integer()
        mineset = 0
        
        do while mineset < minetotals
        'for t = 1 to 10
        
            j = randomvalue(0,Ysize)
            i = randomvalue(0,Xsize)
            console.writeline("mineset {0}, minetotals {1}, i = {2}, j = {3}",mineset,minetotals, i ,j)
            thisplace = mybuttarray(i,j).Tag
            console.writeline("tag 0 = {0}, 1 = {1}, 2 = {2}",thisplace(0),thisplace(1),thisplace(2))
            if thisplace(2) = 0 then
                'set mine here             
                thisplace(2) = 1 
                
                mybuttarray(i,j).Tag = thisplace
                mineset = mineset + 1     
                
                console.writeline("mine {0} set at {1}, {2}",mineset,i,j)
            end if                
        'next t    
        loop
    
    end sub

   
    
    Public Sub butt_clicker(ByVal sender As Object, ByVal e As MouseEventArgs)
        
        if timestopped then 
            timestopped = false
            aTimer.Enabled = True
        end if

        
        dim zis as button
        zis = CType(sender, System.Windows.Forms.Button)
          
        
        
        console.writeline("clicked button i = {0}, j = {1}, mineflag = {2}",zis.Tag(0), zis.Tag(1),zis.Tag(2))
        
        console.writeline("button = {0}", e.Button)
        
        select case e.Button 
            case MouseButtons.Right
                if zis.tag(3) = 1 then exit select ' do nothing on open field
                
                console.writeline("do = {0}", "Flag")
                
                if zis.tag(4) = 1 then zis.tag(4) = 0 else zis.tag(4) += 1 
                ' circle states (0=empty)->(1=flag)->(0=empty)
                
                console.writeline("set flag to {0}", zis.tag(4))
                
                select case cint(zis.tag(4))
                    case 0
                        zis.Image = Nothing
                    case 1
                        zis.Image = smile
                    case 2
                        zis.Image = cat
                        
                end select
                
            case MouseButtons.Left
                if not zis.tag(4) = 0 then exit select ' do nothing on flagged field
                if not zis.tag(3) = 0 then exit select ' do nothing on open field
                
                console.writeline("do = {0}", "Check")
         
                if zis.tag(2) = 1 then 
                    console.writeline ("BANG BANG !")
                    zis.tag(3) = 1
                    zis.FlatStyle = FlatStyle.Popup
                    zis.Image = dog
                else
                    
                    call checklocalmines(zis)
                
                end if               
                
                'check game end = open all minesfree place
                
                dim s,r as integer
                dim chks as integer = fsize
                dim bangs as integer = 0
                dim totmines as integer = 0
                
                for s = 0 to Xsize
                    for r = 0 to Ysize
                    
                        totmines += mybuttarray(s,r).tag(2)
                        chks -= mybuttarray(s,r).tag(2) + mybuttarray(s,r).tag(3) - mybuttarray(s,r).tag(2) * mybuttarray(s,r).tag(3) 'open or mined
                        bangs += mybuttarray(s,r).tag(2) * mybuttarray(s,r).tag(3) ' open and mined
                        
                    next r
                next s
                
                console.writeline("left to free = {0}, banged = {1}", chks, bangs)
                Label1.Text= String.Format("Left to disarm = {0}, life spent = {1}", chks, bangs)
                
                if chks=0 then 'end game routines
                    console.writeline("Game over! Life spent = {0}", bangs)
                    elapsedtime = DateTime.Now.Subtract(_elapseStartTime)
                    console.writeline(String.Format("{0}hr : {1}min : {2}sec", elapsedtime.Hours, elapsedtime.Minutes, elapsedtime.Seconds))
                    

                    Label1.Text= String.Format("Игра окончена. Мин взорвалось = {0}. Мин разминировано = {1}",  bangs, totmines - bangs)
                    
                    dim restext as string = String.Format("Общее время : {0} ч : {1} м : {2} с : {3} мс", elapsedtime.Hours, elapsedtime.Minutes, elapsedtime.Seconds, elapsedtime.Milliseconds)
                    
                    Label2.Text = restext
                    
                    timestopped = true
                    aTimer.Enabled = false

                    for s = 0 to Xsize
                        for r = 0 to Ysize
                    
                            if mybuttarray(s,r).tag(2)=1 and mybuttarray(s,r).tag(3)=0 then
                                mybuttarray(s,r).tag(4)=1
                                mybuttarray(s,r).Image = smile
                            end if
                            
                            removehandler mybuttarray(s,r).MouseDown, addressof butt_clicker 
                        next r
                    next s
                
                    Msgbox(String.Format("Игра окончена! \n\nМин взорвалось = {0} ({1}). Мин разминировано = {2} ({3}). \n\n\n{4}\n\nПлощадь поля = {5}\n\nКоличество мин = {6}\n\nЗаминированность поля (мины/площадь) = {7}",  bangs, FormatPercent(bangs/totmines), totmines - bangs, FormatPercent((totmines - bangs)/totmines), restext, fsize, totmines, FormatPercent(totmines/fsize) ).Replace("\n", Environment.NewLine), , "Результаты игры")
                    
                    exit sub
                
                end if
                
            case MouseButtons.Middle
                console.writeline("do = {0}", "Auto")
                
                dim i,j as integer
                for i = math.max(0, zis.tag(0)-1) to math.min(Xsize, zis.tag(0)+1) 
                    for j = math.max(0, zis.tag(1)-1) to math.min(Ysize, zis.tag(1)+1) 
                        if mybuttarray(i,j).tag(3)=0 and mybuttarray(i,j).tag(4)=0 then
                            console.writeline("autoclick on i = {0}, j = {1}",i,j)
                            call butt_clicker(mybuttarray(i,j), New MouseEventArgs(Windows.Forms.MouseButtons.Left, 1, 0, 0, 0))                            
                        end if
                    next j
                next i
                
                
                
        end select

        

    end sub

    
    sub checklocalmines(zis as System.Windows.Forms.Button)

        dim i,j,localmines as integer
        localmines = 0
        for i = math.max(0, zis.tag(0)-1) to math.min(Xsize, zis.tag(0)+1) 
            for j = math.max(0, zis.tag(1)-1) to math.min(Ysize, zis.tag(1)+1) 
                if mybuttarray(i,j).tag(2) = 1 then localmines += 1
            next j
        next i

        zis.text = cstr(localmines)
        zis.FlatStyle = FlatStyle.Popup
        zis.tag(3) = 1



        if localmines = 0 then 
            for i = math.max(0, zis.tag(0)-1) to math.min(Xsize, zis.tag(0)+1) 
                for j = math.max(0, zis.tag(1)-1) to math.min(Ysize, zis.tag(1)+1) 
                    if mybuttarray(i,j).tag(3)=0 then
                        console.writeline("autoclick on i = {0}, j = {1}",i,j)
                        call checklocalmines(mybuttarray(i,j))
                        'call butt_clicker(mybuttarray(i,j), New MouseEventArgs(Windows.Forms.MouseButtons.Left, 1, 0, 0, 0))                            
                        
                    end if
                next j
            next i
        end if
        
    end sub
    
    public sub b_click(ByVal sender as object, byval e as eventargs)
        console.writeline("b_click executed" & vbCrlf & "on object : " & sender.tostring() & vbCrlf & vbTab  & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
       ' MyControlArray.Remove()
       if not timestopped then d.PerformClick()
       
       call gobuttons

       call settimer
    end sub
    
    public sub c_click(ByVal sender as object, byval e as eventargs)
       console.writeline("c_click executed" & vbCrlf & "on object : " & sender.tostring() & vbCrlf & vbtab  & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))       

        call setmines
    end sub
    
    public sub d_click(ByVal sender as object, byval e as eventargs)
       console.writeline("d_click executed" & vbCrlf & "on object : " & sender.tostring() & vbCrlf & vbtab  & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))       
       timestopped = not timestopped
       aTimer.Enabled = not aTimer.Enabled
    end sub    
    
    
    ' Specify what you want to happen when the Elapsed event is  
    ' raised. 
    
    public Sub OnTimedEvent(source As Object, e As ElapsedEventArgs)
        Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime)
        elapsedtime = DateTime.Now.Subtract(_elapseStartTime)
        Label2.Text= String.Format("Время потрачено : {0} hr : {1} min : {2} sec ", elapsedtime.Hours, elapsedtime.Minutes, elapsedtime.Seconds)
        
        'call dogmove
    End Sub 
    
   
    

    
End Class

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Class testSettings
Inherits System.Windows.Forms.Form

    private WithEvents  textbox1 as New textbox()
    private WithEvents textbox2 as New textbox()
    private WithEvents textbox3 as New textbox()
    
    private d as New Button()
    
    Dim Label1 as New Label()
    Dim Label2 as New Label()

	
	Dim toolTip1 As New ToolTip()

    Public Sub New()
    
        MyBase.New()
        MyBase.Topmost = True
        MyBase.Text = "Это Заголовок формы: Настройки Игры"
        MyBase.Size = new System.Drawing.Size(420,420)
        
       ' MyControlArray = New ButtonArray(Me)
       ' MyControlArray.AddNewButton()
       ' MyControlArray(0).BackColor = System.Drawing.Color.Red
        
        textbox1.Dock = DockStyle.Top
        textbox1.Text = cstr(Test.Xsize) + 1
        'addhandler b.Click, addressof b_click    

        textbox2.Dock = DockStyle.Top
        textbox2.Text = cstr(Test.Xsize) + 1
        'addhandler c.Click, addressof c_click         

        textbox3.Dock = DockStyle.Top
        textbox3.Text = cstr(Test.minetotals)        
        
        d.Dock = DockStyle.Top
        d.Text = "Начать игру с выбранными установками"
        addhandler d.Click, addressof d_click      
                

        Label1.BackColor = System.Drawing.Color.LightGray
        Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        'Label1.Width = 60
        Label1.Text = "Это метка Label1: Таймер"
        'Label1.location = new System.Drawing.Point(20, 77)
        Label1.Dock = DockStyle.Top
    
    
        Label2.BackColor = System.Drawing.Color.DarkGray
        Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Label2.Text = "Это метка Label2: Статус игры"
        Label2.Dock = DockStyle.Top    
    
    
        MyBase.Controls.Add(Label2)
        MyBase.Controls.Add(Label1)
 
 
        MyBase.Controls.Add(d)
        MyBase.Controls.Add(textbox3)
        MyBase.Controls.Add(textbox2)
        MyBase.Controls.Add(textbox1)


        'addhandler textBox1.Validated, addressof TextBox1_Validating 
		
        MyBase.Show()                                
            
        'b.PerformClick()  
     End sub      


        Private Sub TextBox1_MouseHover(sender As Object, e As System.EventArgs) Handles TextBox1.MouseHover
            dim target as textbox = CType(sender, TextBox)            
            ' Update the mouse event label to indicate the MouseHover event occurred.
            ToolTip1.Show("Тут должна быть ширина поля", target, 30, 0, 4500)         
        end sub
        
        Private Sub TextBox2_MouseHover(sender As Object, e As System.EventArgs) Handles TextBox2.MouseHover
            dim target as textbox = CType(sender, TextBox)            
            ' Update the mouse event label to indicate the MouseHover event occurred.
            ToolTip1.Show("Тут должна быть высота поля", target, 30, 0, 4500)         
        end sub        
        
        Private Sub TextBox3_MouseHover(sender As Object, e As System.EventArgs) Handles TextBox3.MouseHover
            dim target as textbox = CType(sender, TextBox)            
            ' Update the mouse event label to indicate the MouseHover event occurred.
            ToolTip1.Show("Тут должно быть количество мин на поле", target, 30, 0, 4500)         
        end sub        

				
		Private Sub TextBox123_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TextBox1.Validating, TextBox2.Validating , TextBox3.Validating
			e.Cancel = Not IsTextInteger(CType(sender, TextBox))
		End Sub
        
		Private Function IsTextInteger(target As TextBox) As Boolean
			If target.TextLength = 0 Then Return True
			If Integer.TryParse(target.Text, Nothing) Then
				ToolTip1.SetToolTip(target, String.Empty)
				Return True
			Else
				ToolTip1.Show("Text entered must be an integer.", target, 0, target.Height, 5000)
				target.SelectAll()
				Return False
			End If
		End Function			

  
        
        
		public sub d_click(ByVal sender as object, byval e as eventargs)
		   console.writeline("d_click executed" & vbCrlf & "on object : " & sender.tostring() & vbCrlf & vbtab  & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))       
			'System.Windows.Forms.Application.Run(New Test())
            
			Test.Xsize = cint(TextBox1.text) - 1
			Test.Ysize = cint(TextBox2.text) - 1
            Test.minetotals = cint(TextBox3.text)
            if Test.minetotals/( Test.Xsize * Test.Ysize) > 0.99 then 
                msgbox(String.Format("Слишком много мин! Мины не должны занимать больше 99% поля."),,"Предупреждение!")
            else
                dim T as new Test
            end if
			'showdialog(T)		
			
		end sub    		
		
            



end class


'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

module Module1
    Public  Sub Main()
        'Dim T as New Test
        
        'T = New Test()
        'System.Windows.Forms.Application.Run(New Test())
		System.Windows.Forms.Application.Run(New testSettings())
    End Sub
	
end module	

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
