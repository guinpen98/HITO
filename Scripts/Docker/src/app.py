from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import subprocess

app = FastAPI()

class Text(BaseModel):
    text: str

@app.post('/analyze/')
async def analyze_text(text: Text):
    if not text.text:
        raise HTTPException(status_code=400, detail="No text provided")

    # Juman++で形態素解析
    jumanpp_result = subprocess.run(
        ['echo', text.text, '|', 'jumanpp'],
        capture_output=True, text=True, shell=True
    ).stdout

    # KNPで構文解析
    knp_result = subprocess.run(
        ['echo', jumanpp_result, '|', 'knp'],
        capture_output=True, text=True, shell=True
    ).stdout

    return {
        "jumanpp": jumanpp_result,
        "knp": knp_result
    }
