FROM python:3.9-slim

# 必要なパッケージのインストール
RUN apt-get update && apt-get install -y \
    wget \
    build-essential \
    cmake \
    swig \
    zlib1g-dev

# Juman++のインストール
RUN wget https://github.com/ku-nlp/jumanpp/releases/download/v2.0.0-rc4/jumanpp-2.0.0-rc4.tar.xz && \
    tar xJvf jumanpp-2.0.0-rc4.tar.xz && \
    cd jumanpp-2.0.0-rc4 && \
    mkdir build && cd build && \
    cmake .. && \
    make install && \
    cd / && rm -rf jumanpp-2.0.0-rc4*

# KNPのインストール
RUN wget http://nlp.ist.i.kyoto-u.ac.jp/nl-resource/knp/knp-4.20.tar.bz2 && \
    tar jxvf knp-4.20.tar.bz2 && \
    cd knp-4.20 && \
    # config.guess と config.sub を最新版に更新
    wget -O config.guess 'http://git.savannah.gnu.org/gitweb/?p=config.git;a=blob_plain;f=config.guess;hb=HEAD' && \
    wget -O config.sub 'http://git.savannah.gnu.org/gitweb/?p=config.git;a=blob_plain;f=config.sub;hb=HEAD' && \
    wget -O CRF++-0.58/config.guess 'http://git.savannah.gnu.org/gitweb/?p=config.git;a=blob_plain;f=config.guess;hb=HEAD' && \
    wget -O CRF++-0.58/config.sub 'http://git.savannah.gnu.org/gitweb/?p=config.git;a=blob_plain;f=config.sub;hb=HEAD' && \
    wget -O juman/config.guess 'http://git.savannah.gnu.org/gitweb/?p=config.git;a=blob_plain;f=config.guess;hb=HEAD' && \
    wget -O juman/config.sub 'http://git.savannah.gnu.org/gitweb/?p=config.git;a=blob_plain;f=config.sub;hb=HEAD' && \
    ./configure && \
    make && \
    make install && \
    cd / && rm -rf knp-4.20*

# 作業ディレクトリの設定
WORKDIR /app

# Python依存ライブラリのインストール
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# アプリケーションのコードをコピー
COPY src/ .

# アプリケーションの起動コマンド
CMD ["uvicorn", "app:app", "--host", "0.0.0.0", "--port", "8000"]
