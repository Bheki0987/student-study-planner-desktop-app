   M S B A M L   `   `   `  ����N  JStudentStudyPlanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:StudentStudyPlannerStudentStudyPlanner  5      %    StudentStudyPlanner.AddTaskPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      3local!clr-namespace:StudentStudyPlanner     5      X TXceed.Wpf.Toolkit, Version=4.6.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      .��5      � ���#F5F5F5.+�5   
   �� 
�������  �� CornerRadius$  10��6$   $��20q�66    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5	         
BlurRadius$ 10Q�6#      ShadowDepth$ 0Q�63      Opacity$	 0.1Q�6C   ._�5      � $��20q�6    � RowDefinitions 5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $��* �6$    � ColumnDefinitions 5      �� 5      $
��Auto �6'   �� 5      $��* �6'   .+�5      �� $��Add New Task��6   $��2��60   $��24(�6D   $
��Bold"�6R   $��0,0,0,20q�6d   $��Center=�6v   �� 5      $��
Task Name:��6      ModernTextBlockStyle#��[  6.   �� 5      -   5         ��Name$ TaskNameTextBox��5   *   $��1��6     ModernTextBoxStyle#��[ 6C   �� 5       $��1��6   $��Date:��6)   #��[  66   (  "System.Windows.Controls.DatePicker  5!      -   5          Name$ TaskDatePicker��5!   :   $��1��6   $��1��6*   $��0,5,0,10q�6R   �� 5#      $��2��6   $��	Due Time:��6)   #��[  6:   �� 5$      $��2��6   $��1��6*   	 �� Orientation$	 
Horizontal=�6:   .+�5%      �� -   5        $ TaskHourTextBox��5%      $��40��67   #��[ 6B   $��0,5,5,10q�6n   �� 5&      $��:��6    $��Center=�6)   $��0,5,5,10q�6D   �� 5'      -   5        $ TaskMinuteTextBox��5'      $��40��69   #��[ 6D   $��	0,5,10,10q�6p   �� 5(      -   5        
 ��Name$
 TaskTimePeriodComboBox��5(      $��60��6?     ModernComboBoxStyle#��[ 6J   .@�5)      �� $��AM��6'   �� 5*      $��PM��6'   5+      5,      �� 5.      $��3��6   $��	Priority:��6)   #��[  6:   �� 5/      -   5        $
 TaskPriorityComboBox��5/   8   $��3��6   $��1��6(   #��[ 6V   .@�50      �� $	��Low��6#   �� 51      $��Medium��6#   �� 52      $
��High��6#   53      �� 55      $��4��6   $��	Category:��6)   #��[  6:   �� 56      -   5        $
 TaskCategoryComboBox��56   8   $��4��6   $��1��6(   #��[ 6V   .@�57      �� $��Study��6#   �� 58      $��Homework��6#   �� 59      $
��Exam��6#   �� 5:      $��Project��6#   �� 5;      $��Other��6#   5<      �� 5>      $��5��6   $��Color:��6)   #��[  67   #  Xceed.Wpf.Toolkit.ColorPicker  5?      -   5          Name$ TaskColorPicker��5?   @   $��5��6#   $��1��60   6Y   �� 5A      $��7��6   $��2��6*   $	 
Horizontal=�6>   $��Right=�6W   $��0,20,0,0q�6s   .+�5B      �� -	   5        $��Cancel��5B      $	��100��6.   $��35��6:   $��0,0,10,0q�6F     ModernButtonStyle#��[ 6m   ���#FF52526�   �� 5C      -
   5        $
��Save��5C      $	��100��6,   $��35��68   #��[ 6[   5D      5E      5F      5G      5H      