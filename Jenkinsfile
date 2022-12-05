pipeline {
  
  agent any

  environment {
    MSBUILD = "C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Msbuild\\Current\\Bin\\MSBuild.exe"
    CONFIG = 'Debug'
    PLATFORM = 'x86'
  }
  
  stages {

    // stage('Update manifest version') {
    //   steps {
    //       powershell '''
    //         $manifest = "01_VisualStudio\\WinForms.App\\WinForms.Packaging\\Package.appxmanifest"     
    //         [xml]$xmlDoc = Get-Content $manifest
    //         $version = $xmlDoc.Package.Identity.Version
    //         $trimmedVersion = $version -replace '.[0-9]+$', '.'
    //         $xmlDoc.Package.Identity.Version = $trimmedVersion + ${env:BUILD_NUMBER}
    //         $xmlDoc.Save($manifest)
    //       '''
    //   }
    // }
    stage('Build') {
      steps {
        bat "dotnet restore C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList\\ToDoList.csproj"

        bat "\"${MSBUILD}\" C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln /p:Configuration=${env.CONFIG} /p:AppxBundlePlatforms=${env.PLATFORM}  /p:AppxBundle=Never /p:UapAppxPackageBuildMode=Sideloading  /p:AppxPackageSigningEnabled=false"
        
      }
      post{
          always {
           archiveArtifacts artifacts: '**/*.msix', followSymlinks: false
          }
      }
    }
  }
}