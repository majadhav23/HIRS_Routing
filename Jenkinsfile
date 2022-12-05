pipeline {
    agent any
    environment {
        dotnet = 'C:\\Program Files\\dotnet\\dotnet.exe'
    }
    stages {
        // stage('Checkout Stage') {
        //     steps {
        //         git credentialsId: '5ba5e0da-116a-47df-8e8c-639f4654358c', url: 'https://github.com/majadhav23/HIRS_Routing.git', branch: 'main'
        //     }
        // }
        stage('Build Stage') {
            steps {
                echo 'Build Stage...'
                bat 'dotnet clean C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln'
                bat "dotnet build C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln"
                //bat 'C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln --configuration Release'
            }
        }
        stage('Code Review') {
            steps {
                echo 'Code Review using Sonarqube'
              //  bat 'dotnet test %WORKSPACE%\\TestProject1\\TestProject1.csproj'
            }
        }
        stage("Automation Test") {
            steps {
                echo 'Automation testing'
                //bat 'dotnet build %WORKSPACE%\\JenkinsWebApplicationDemo.sln /p:PublishProfile=" %WORKSPACE%\\JenkinsWebApplicationDemo\\Properties\\PublishProfiles\\FolderProfile.pubxml" /p:Platform="Any CPU" /p:DeployOnBuild=true /m'
            }
        }
        stage('Code Deploy') {
            steps {
                //Deploy application on IIS
                echo 'Code Deployment'
                bat "dotnet publish C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln"
                // bat 'net stop "w3svc"'
                // bat '"C:\\Program Files (x86)\\IIS\\Microsoft Web Deploy V3\\msdeploy.exe" -verb:sync -source:package="%WORKSPACE%\\JenkinsWebApplicationDemo\\bin\\Debug\\net6.0\\JenkinsWebApplicationDemo.zip" -dest:auto -setParam:"IIS Web Application Name"="Demo.Web" -skip:objectName=filePath,absolutePath=".\\\\PackagDemoeTmp\\\\Web.config$" -enableRule:DoNotDelete -allowUntrusted=true'
                // bat 'net start "w3svc"'
            }
        }
    }
}

// pipeline {
  
//   agent any

//   environment {
//     MSBUILD = "C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Msbuild\\Current\\Bin\\MSBuild.exe"
//     CONFIG = 'Release'
//     PLATFORM = 'x86'
//   }
  
//   stages {

//     // stage('Update manifest version') {
//     //   steps {
//     //       powershell '''
//     //         $manifest = "01_VisualStudio\\WinForms.App\\WinForms.Packaging\\Package.appxmanifest"     
//     //         [xml]$xmlDoc = Get-Content $manifest
//     //         $version = $xmlDoc.Package.Identity.Version
//     //         $trimmedVersion = $version -replace '.[0-9]+$', '.'
//     //         $xmlDoc.Package.Identity.Version = $trimmedVersion + ${env:BUILD_NUMBER}
//     //         $xmlDoc.Save($manifest)
//     //       '''
//     //   }
//     // }
//     stage('Build') {
//       steps {
//         //bat "dotnet restore C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList\\ToDoList.csproj"
//         bat "dotnet build C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln"
//         //bat "\"${MSBUILD}\" C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln /p:Configuration=${env.CONFIG} /p:AppxBundlePlatforms=${env.PLATFORM}  /p:AppxBundle=Never /p:UapAppxPackageBuildMode=Sideloading  /p:AppxPackageSigningEnabled=false"
//         //bat "\"${MSBUILD}\" C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln /nologo /nr:false  /p:platform=\"x86\" /p:configuration=\"release\" /t:clean;restore;rebuild"
//       }
//       post{
//           always {
//            archiveArtifacts artifacts: '**/*.msix', followSymlinks: false
//           }
//       }
//     }
//   }
// }