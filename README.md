# MediaControls Is a Custom UI for Dotnet Maui MediaElement

Current it works with Navigation Page, Tabbed page, and Shell page.
You can set the page to full screen using MediaControls full screen option
which is the image button in upper right of screen. Controls show at start
of playback.

Clicking or tapping on page will cause the controls to appear for 7 seconds.
In windows double clicking on page will set page to full screen or restore 
the screen to default size. If you have MediaControls as only element on page 
full screen controls will work properly.

You don't need to worry about tab bar or nav bar it will be hidden or shown
with state preserved so that if you go full screen then back again it will
show the previous state upon restore default page size.
