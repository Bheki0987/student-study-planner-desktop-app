   M S B A M L   `   `   `  ����N  JStudentStudyPlanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:StudentStudyPlannerStudentStudyPlanner  5      &     StudentStudyPlanner.SchedulePage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      3local!clr-namespace:StudentStudyPlanner     5      .��5      � ���#F5F5F5  � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5
      $
��Auto �6   .+�5   
   �� $��Schedule��6   $��28(�6$   $
��Bold"�62   $��20q�6D   $��Center=�6P   ���#3333336m   �� 5   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��
20,0,20,10q�6;   .+�5      �� -   5         ��Name$ FilterComboBox��5      $	��150��6/   $��0,0,10,0q�6;   .@�5      �� $��	All Tasks��6   �� 5      $��Today's Tasks��6   �� 5      $��This Week's Tasks��6   �� 5      $��Completed Tasks��6   �� 5      $��Incomplete Tasks��6   5      �� 5      -   5        $ SortComboBox��5      $	��150��6-   $��0,0,10,0q�69   .@�5      �� $��Date (Ascending)��6   �� 5      $��Date (Descending)��6   �� 5      $��Priority (High to Low)��6   �� 5      $��Priority (Low to High)��6   5      �� 5      -   5        $��Apply��5      $	��100��6G      ModernButtonStyle#��[  6S   5      �� 5    
   $��2��6   $��
20,0,20,20q�6$   .��5!      �� -   5         ��Name$ TasksItemsControl��5!      5#      5$      �� 5&   
   -   5        $��3��5&      $��
+ ADD TASK��6   $	��130��63   $��40��6?   $��20q�6K   #��[  6m   ���#4CAF50$��Center=�6�   5'      5(      