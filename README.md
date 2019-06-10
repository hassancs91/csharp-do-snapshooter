# csharp-do-snapshooter
Simple c# application that allows you to manage your Digital Ocean Snapshots and automate Tasks

Hey Friends!

If you have used Digital Ocean Cloud Services, you then know that you can take snapshots of your machine as a backup.
I created this small application that utlizez the Digital Ocean API in a small C# windowsforms app using curl under windows and simple
process then I read the json results and showed them is a clear view in the form.

<h3>The application allows you to:<h3>
<ul>
  <li>Enumerate your Droplets</li>
 <li>Get Snapshots per droplet</li>
 <li>Create Snapshots in bulk</li>
 <li>Delete Snapshots in bulk</li>
 <li>Get all Snapshots and estimate your monthly cost</li>
</ul>


<h3>How to run?<h3>

<p>You need to download the curl executable for windows and place it besire the applciation executable file.</p>

And then the application will run normally.
<br>

<a href="https://curl.haxx.se/windows/" target="_blank">Click Here to Download Curl For Windows</a>

<p>If the Link is not working please contact me to update it</p>


<h3>Example of utilizing the curl executable in the application<h3>
  
            listView1.Items.Clear();
            string jsonreturn = string.Empty;
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = "crl.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                Arguments =
                    "-X GET -H \"Content-Type: application/json\" -H \"Authorization: Bearer " + textBoxKEY.Text + "\" \"https://api.digitalocean.com/v2/droplets\""
            };

            using (var exeProcess = Process.Start(startInfo))
            {

                if (exeProcess != null) jsonreturn = exeProcess.StandardOutput.ReadToEnd();
            }



            dynamic dynObj = JsonConvert.DeserializeObject(jsonreturn);
            foreach (var data in dynObj.droplets)
            {

                ListViewItem li = new ListViewItem();
                li.Text = data.id.ToString();
                li.SubItems.Add(data.name.ToString());
                listView1.Items.Add(li);


            }

<br>

<h3>How it can be improved</h3>
1- Converting this applciation to a windows service or API<br>
2- Adding scheduling so  snapshoots can be fully automated<br>
3- Adding log Files

<h3>Need Help?</h3>
Email: Support@h-educate.com
Website Questions: https://h-educate.com/ask-question/ <br>
Facebook Page: https://www.facebook.com/AdvancedITEducation/


