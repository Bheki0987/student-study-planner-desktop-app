   M S B A M L   `   `   `  ����N  JStudentStudyPlanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:StudentStudyPlannerStudentStudyPlanner  5   	   $    StudentStudyPlanner.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   3local!clr-namespace:StudentStudyPlanner     5   	     � Title$  Student Study Planner��5   	   $	��650��6'   $
��1000��64    � 	Resources 5         ImprovedNavButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� CornerRadius �� Triggers	 P� Property
 P� Value �� 
TargetName3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   
BlurRadius   Opacity   Color��%�  &	        �� 5   
   " ��6/   .�5      ��  � ��
 ���� 6+   �� 5	       � ��
 �����6+   �� 5
       � ��$ 0q�60   �� 5       � ��$ 50��6'   �� 5       � ��$ 16(�6)   �� 5       � ��$ SemiBold"�6+   �� 5       � ��$
 10,5q�6'   �� 5       � ��$
 15,0q�6(   �� 5       � ��$
 Left=�6;   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ border��6!   #��z��5   !   #��z��5   !   #��z��5   !   $ 25��5   !   ._�5      � .+�5   "   �� #��z��63   #��z��5   3   #��z��5   3   5      5       5      P� 5       	 �@J�IsMouseOver
 . . �5!   "   ��  � �� �#405C796?   �� 5"   "   $ border��6)    �@J�Effect 5#   &     5$   *   $ 0Q�6;   $ 10Q�6K   $	 0.5Q�6[   $ Black��6i   5&   #   5'      P� 5(      	 � ��
 . . �5)   "   ��  � �� �#34495E6?   5*      5,      5.      5/      50      .��52      �  � ColumnDefinitions 53   
   �� 54      $	��250 �6   �� 55      $��* �6   .+�58   
   � $��0��6   ���#2C3E50 � RowDefinitions 59      �� 5:      $
��Auto �6    �� 5;      $��* �6    �� 5<      $
��Auto �6    .+�5?      �� $��Study Planner��6   $��28(�5@      $
��Bold"�5A      
�������$��Center=�5C      $��	0,30,0,30q�5D      �� 5F      $��1��6   .+�5G      �� -   5        $��📅 Schedule��5G      #��[  6J   �� 5H      -   5        $��🗓️ Calendar��5H      #��[  6K   �� 5I      -   5        $��
📝 Notes��5I      #��[  6D   �� 5J      -   5        $��⏱️ Study Timer��5J      #��[  6O   �� 5K      -   5        $��📊 Progress Tracking��5K      #��[  6[   5L      �� 5N      -   5        $��2��5N      $��🚪 Logout��5O      #��[  5Q      ���#C0392B$��10,20,10,30q�5S      5T       � 5V   
   -   5          �Name$ 	MainFrame��5V      $��1��6#     � NavigationUIVisibility$ Hidden=�63   5W      5X      