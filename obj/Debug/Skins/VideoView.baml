   M S B A M L   `   `   `  ����M  IAudioVideoParcerVk, Version=0.0.0.1, Culture=neutral, PublicKeyToken=nullK*clr-namespace:AudioVideoParcerVk.ViewModelAudioVideoParcerVk.ViewModel  5      (    "AudioVideoParcerVk.Skins.VideoView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      n jTelerik.Windows.Controls.GridView, Version=2016.1.112.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2016.1.112.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7j fTelerik.Windows.Controls.Data, Version=2016.1.112.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2016.1.112.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e	 aTelerik.Windows.Controls, Version=2016.1.112.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation     	 5      >local*clr-namespace:AudioVideoParcerVk.ViewModel     	 5        � DataContext  5	      �� *Video+   Locator �� Source# [  .��5
      � $��	0,-62,0,0q�6   $	��653��6   $��Bottom=�6+   .+�5   
   �� $��Выгрузить в CSV��6   $
��Left=�6I   $	��Top=�6e   $	��103��6}   $��	0,196,0,0q�6�   ��6+   �� *	UnloadCSV+6�   �� 5   
   $%��Очистить таблицу��6   $
��Left=�6M   $��108,196,0,0q�6h   $	��Top=�6}   $	��125��6�   ��6,   �� *ClearRadGrid+6�   	� 5   
   $��739,55,103,0q�6@   $	��Top=�6V   $��24��6n    d� 	IsChecked 6   �� *ComboCheckOneSearch+6n   	� 5   
   $��739,137,103,0q�6A   $	��Top=�6X   $��24��6p    6   �� *ComboCheckManySearch+6p   �� 5   
   
���   �$&�� Получать только id��6&   $
��Left=�6d   $��516,203,0,0q�6   $	��Top=�6�    6C   �� *
CheckGetId+6�   �� 5   
   $1��+Поисковой запрос к vk Video��6   $��	0,0,0,550q�6c   $
��Left=�6v   $	��734��6�   $��2��6�   {�68   �� *GroupBoxOneIsEnabled+.��5      � .+�5      �� $
��Left=�6   $��23��65    �� TextWrapping$
 Wrap=�6A   $	��Top=�6r   $	��506��6�   $��	10,10,0,0q�6�   ��6U   �� *SearchVideo+6�   �� 5      $#��Получить данные��6   $
��Left=�6s   $	��Top=�6�   $	��119��6�   $��
521,10,0,0q�6�   ��63   �� *GetSearchValue+{�6V   �� *Active+6�   �� 5      $
��Stop��6   $
��Left=�6I   $��
645,10,0,0q�6d   $	��Top=�6x   $��75��6�   ��6(   �� *StopGetValue+6�   5      5      �� 5   
   $^��XСписок поисковых запросов vk Video c разделителем ';'��6   $��0,103,0,462q�6~   $
��Left=�6�   $	��734��6�   $��2��6�   {�6R   �� *GroupBoxManyIsEnabled+.��5      � .+�5      �� 
���   �$
��Left=�6W   $	��Top=�6r   $��0,13,0,0q�6�   $	��347��6�   ��6+   �� *FilePathManySearchLabel+6�   �� 5      $#��Получить данные��6   $
��Left=�6w   $	��Top=�6�   $	��119��6�   $��
521,10,0,0q�6�   ��63   �� *GetListSearchValue+{�6Z   �� *Active+6�   �� 5      $
��Stop��6   $
��Left=�6I   $��
645,10,0,0q�6d   $	��Top=�6x   $��75��6�   ��6(   �� *StopGetValue+6�   �� 5      $��Выбрать файл��6   $
��Left=�6W   $	��Top=�6s   $	��104��6�   $��
384,10,0,0q�6�   ��60   �� *OpenFileManySearch+6�   5      5      *  $Telerik.Windows.Controls.RadGridView  5    
   $��0��6   ;  5Telerik.Windows.Controls.GridView.GridViewDataControl   ShowGroupPanel .     AutoGenerateColumns .  $��
0,230,0,28q�6�   $��2��6�   *  $Telerik.Windows.Controls.DataControl   ItemsSource 6^   �� 1   +AudioVideoParcerVk.ViewModel.VideoViewModel   PagedVideoSource# Z    Columns 5!      1  +Telerik.Windows.Controls.GridViewDataColumn  5"      6  0Telerik.Windows.Controls.GridViewBoundColumnBase	   Header$	 User ID��6R   -  'Telerik.Windows.Controls.GridViewColumn
   Width?  9Telerik.Windows.Controls.GridView.GridViewLengthConverter$	
 100 6c      DataMemberBinding 6-   �� *UserId+6c     5#      $	 	User Name��6T   $	
 100 6g    6-   �� *
UserName+6g     5$      $	 Дата рождения��6O   $	
 100 6f    6-   �� *Age+6f     5%      $)	 #Семейное положение��6X   $	
 100 6t    6-   �� *FamilyStatus+6t     5&      $	 
Город��6P   $	
 100 6_    6-   �� *City+6_     5'      $	 Страна��6S   $	
 100 6c    6-   �� *	Country+6c     5(      $	 Phone��6Q   $	
 100 6`    6-   �� *Phone+6`     5)      $%	 Поисковой запрос��6Q   $	
 150 6k    6-   �� *Query+6k   5+      5.      5/      