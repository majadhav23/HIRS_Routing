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
         stage('Unit Testing') {
            steps {
                echo 'Unit Testing...'
                //bat 'dotnet test C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln'
                bat 'dotnet test ${WORKSPACE}\\ToDoList\\ToDoList\\ToDoList.sln'
            }
        }
        stage('Code Review') {
            steps {
                echo 'Code Review using Sonarqube'
                script {
                    def scannerHome = tool 'Sonar';
                    withSonarQubeEnv("Sonar") {
                    bat "${tool("Sonar")}/bin/sonar-scanner \
                    -Dsonar.projectKey=ToDoList \
                    -Dsonar.sources=. \
                    -Dsonar.css.node=. \
                    -Dsonar.exclusions=**/*.java,**/*.js,target/**/* \
                    -Dsonar.host.url=http://localhost:9000 \
                    -Dsonar.login=sqp_5d479b72294fa77e442778dea8e7be7f003025df"
                        }
                    }
            }
        }
        stage('Build Stage') {
            steps {
                echo 'Build Stage...'
                //bat 'dotnet clean C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln'
                bat 'dotnet clean ${WORKSPACE}\\ToDoList\\ToDoList\\ToDoList.sln'
                
                //bat "dotnet build C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln"
                bat 'dotnet build ${WORKSPACE}\\ToDoList\\ToDoList\\ToDoList.sln'
            }
        }
        stage('Code Deploy') {
            steps {
                //Deploy application on IIS
                echo 'Code Deployment'
                bat 'iisreset /stop'
                //bat "dotnet publish C:\\Users\\003VPO744\\Desktop\\SimpleProject\\HIRS_Routing\\ToDoList\\ToDoList.sln -o C:\\inetpub\\wwwroot\\todo"
                bat 'dotnet publish ${WORKSPACE}\\ToDoList\\ToDoList\\ToDoList.sln -o C:\\inetpub\\wwwroot\\todo'
                
                bat 'iisreset'
            }
        }
        stage('Selenium Test') {
            steps {
                bat 'dir'
                bat 'mvn clean'
                bat 'mvn test'
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