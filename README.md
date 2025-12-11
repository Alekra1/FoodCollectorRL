# FoodCollectorRL


## Steps to run

1. Install Unity Hub:

`yay -S unityhub`

then install latest editor there

3. Clone repo:

`git clone git@github.com:Alekra1/FoodCollectorRL.git`

3. Open the repo as project in Unity

4. Install ml-agent package:

- Clone ml-agents repo `git@github.com:Unity-Technologies/ml-agents.git`
- In unity click Window -> Package Management -> Package Manager -> "+" icon -> Install package from disk
- Choose this file: `PATH TO CLONED ML-AGENTS FOLDER/com.unity.ml-agents/package.json`

6. Choose "Training" scene in Scenes folder

7. Install uv:

`curl -LsSf https://astral.sh/uv/install.sh | sh`

6. Create and use virtual enviroment

`cd Training`

`uv sync`

`source .venv/bin/activate`

`mlagents-learn config.yaml --run-id=run0`

7. Open unity and click play


## Run trained model

1. Choose Testing Scene in Scenes folder
2. Click run

The model used here is the one I trained

You can train it yourself and then import in into Models folder. Then choose Agent and in Inspector in Behavior Parameters section choose your model.
