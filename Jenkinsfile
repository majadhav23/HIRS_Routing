pipeline {
    agent any
    environment {
        dotnet = 'C:\\Program Files\\dotnet\\dotnet.exe'
	sonar_token = credentials('SonarQubeToken')
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
                echo "Current workspace is ${WORKSPACE}"
                bat "dotnet test ${WORKSPACE}\\ToDoList\\ToDoList.sln"
            }
        }
        stage('Code Review') {
            steps {
		echo "Token: ${sonar_token}"
                echo 'Code Review using Sonarqube'
                script {
                    def scannerHome = tool 'Sonar';
                    withSonarQubeEnv("Sonar") {
                    bat "${tool("Sonar")}/bin/sonar-scanner \
                    -Dsonar.projectKey=ToDoList \
                    -Dsonar.sources=. \
                    -Dsonar.css.node=. \
                    -Dsonar.exclusions=**/*.java,**/*.js,target/**/* \
                    -Dsonar.host.url=http://localhost:9000"
                        }
                    }
            }
        }
        stage('Build Stage') {
            steps {
                echo 'Build Stage...'
                bat "dotnet clean ${WORKSPACE}\\ToDoList\\ToDoList.sln"
                bat "dotnet build ${WORKSPACE}\\ToDoList\\ToDoList.sln"
            }
        }
        stage('Code Deploy') {
            steps {
                //Deploy application on IIS
                echo 'Code Deployment'
                bat 'iisreset /stop'
                bat "dotnet publish ${WORKSPACE}\\ToDoList\\ToDoList.sln -o C:\\inetpub\\wwwroot\\todo"
                bat 'iisreset'
            }
        }
        stage('Automation Test') {
            steps {
                bat 'dir'
                bat 'mvn clean'
                bat 'mvn test'
            }
        }
    }
}