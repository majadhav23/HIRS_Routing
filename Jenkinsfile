pipeline {
  
  agent any

  environment {
    MSBUILD = "C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Msbuild\\Current\\Bin\\MSBuild.exe"
    CONFIG = 'Release'
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
        //bat "dotnet restore C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList\\ToDoList.csproj"
        bat "dotnet build C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln"
        //bat "\"${MSBUILD}\" C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln /p:Configuration=${env.CONFIG} /p:AppxBundlePlatforms=${env.PLATFORM}  /p:AppxBundle=Never /p:UapAppxPackageBuildMode=Sideloading  /p:AppxPackageSigningEnabled=false"
        //bat "\"${MSBUILD}\" C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln /nologo /nr:false  /p:platform=\"x86\" /p:configuration=\"release\" /t:clean;restore;rebuild"
      }
      post{
          always {
           archiveArtifacts artifacts: '**/*.msix', followSymlinks: false
          }
      }
    }
  }
}